using MySapProject.Application.Interfaces;
using MySapProject.Application.Services;
using MySapProject.Infrastructure.Persistence.Configuration;
using MySapProject.Infrastructure.Persistence.Repository;
using MySapProject.WebApi.Middlewares;
using Serilog;
using Serilog.Events;

namespace MySapProject.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var logBasePath = @"C:\Users\Kamron Noriyev\source\repos\SAP\SAP.WebApi\Log";
        var currentDate = DateTime.Now.ToString("yyyy.MM.dd");
        var logPath = Path.Combine(logBasePath, currentDate, $"log-{currentDate}.txt");

        Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);

        var logFolders = Directory.GetDirectories(logBasePath);
        foreach (var folder in logFolders)
        {
            var folderName = Path.GetFileName(folder);
            if (DateTime.TryParseExact(folderName, "yyyy.MM.dd", null, System.Globalization.DateTimeStyles.None, out var folderDate))
            {
                if ((DateTime.Now - folderDate).TotalDays > 30)
                {
                    Directory.Delete(folder, true);
                }
            }
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                path: logPath,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Infinite, 
                shared: true
            )
            .CreateLogger();


        try
        {
            Log.Information("Dastur ishga tushmoqda");

            var builder = WebApplication.CreateBuilder(args);

           
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File(
                        path: logPath,
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    );
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.RegisterServices();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
            );

            app.UseSession();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Dastur ishga tushishda xatolik yuz berdi");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
