using MySapProject.Application.Interfaces;
using MySapProject.Application.Services;
using MySapProject.Infrastructure.Persistence.Repository;
using System.Net.Http.Headers;

namespace MySapProject.Infrastructure.Persistence.Configuration;

public static class ConfigureDependencies
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        var baseUrl = builder.Configuration["SapConnection:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException("SAP BaseUrl konfiguratsiyada topilmadi.");

        builder.Services.AddHttpClient<ILoginRepository, LoginRepository>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        builder.Services.AddHttpClient<ILogoutRepository, LogoutRepository>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });

        builder.Services.AddScoped<ILogoutService, LogoutService>();
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddSingleton<ISessionStorage, SessionStorage>();
    }
}
