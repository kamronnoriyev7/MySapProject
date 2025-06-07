using MySapProject.Application.Interfaces;
using MySapProject.Application.Services;
using MySapProject.Infrastructure.Persistence.Repository;
using System.Net.Http.Headers;

// Ensure correct using statement for BusinessPartnerRepository if it's different
// Assuming BusinessPartnerRepository is in MySapProject.Infrastructure.Persistence.Repository
// and BusinessPartnerService is in MySapProject.Application.Services

namespace MySapProject.Infrastructure.Persistence.Configuration; // Namespace seems to be Infrastructure.Persistence.Configuration based on original

public static class ConfigureDependencies
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        var baseUrl = builder.Configuration["SapConnection:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException("SAP BaseUrl konfiguratsiyada topilmadi."); // Uzbek: SAP BaseUrl not found in configuration.

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

        // Add HttpClient for BusinessPartnerRepository
        builder.Services.AddHttpClient<IBusinessPartnerRepository, BusinessPartnerRepository>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        // Register Application Services
        builder.Services.AddScoped<ILogoutService, LogoutService>();
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<IBusinessPartnerService, BusinessPartnerService>(); // Added BusinessPartnerService

        // Register other singletons or scoped services
        builder.Services.AddSingleton<ISessionStorage, SessionStorage>();

        // Note: Repositories like LoginRepository, LogoutRepository, and BusinessPartnerRepository are typically registered
        // via AddHttpClient<Interface, Implementation>, which handles their lifecycle (transient by default with HttpClientFactory).
        // If they didn't require HttpClient, they'd be registered like:
        // builder.Services.AddScoped<IBusinessPartnerRepository, BusinessPartnerRepository>();
    }
}
