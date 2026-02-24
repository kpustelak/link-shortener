using LinkShortener.API.Interface;
using LinkShortener.API.Services;

namespace LinkShortener.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddLinkServices(this IServiceCollection services)
    {
        services.AddScoped<ILinkService, LinkService>();
        services.AddScoped<IPasswordService, PasswordService>();
        
        return services;
    }
}