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

        builder.Services.AddTransient<Application.Services.HttpClient.HttpClientHandler>();  

        builder.Services.AddHttpClient<ILoginRepository, LoginRepository>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddHttpMessageHandler<Application.Services.HttpClient.HttpClientHandler>(); 

        builder.Services.AddHttpClient<ILogoutRepository, LogoutRepository>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddHttpMessageHandler<Application.Services.HttpClient.HttpClientHandler>();

        builder.Services.AddHttpClient<IBusinessPartnerRepository, BusinessPartnerRepository>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddHttpMessageHandler<Application.Services.HttpClient.HttpClientHandler>();

        builder.Services.AddHttpClient<IItemRepository, ItemRepository>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
            .AddHttpMessageHandler<Application.Services.HttpClient.HttpClientHandler>();

        builder.Services.AddHttpClient<IEmployeeRepository, EmployeeRepository>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
            .AddHttpMessageHandler<Application.Services.HttpClient.HttpClientHandler>();
        
        builder.Services.AddHttpClient<IIncomingPaymentRepository, IncomingPaymentRepository>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
            .AddHttpMessageHandler<Application.Services.HttpClient.HttpClientHandler>();

        builder.Services.AddHttpClient<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
           .AddHttpMessageHandler<Application.Services.HttpClient.HttpClientHandler>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ISessionStorage, SessionStorage>();

        builder.Services.AddScoped<ILogoutService, LogoutService>();
        builder.Services.AddScoped<ILoginService, LoginService>();

        builder.Services.AddScoped<IBusinessPartnerService, BusinessPartnerService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IItemService, ItemService>();
        builder.Services.AddScoped<IIncomingPaymentService, IncomingPaymentService>();
        builder.Services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
    }

}
