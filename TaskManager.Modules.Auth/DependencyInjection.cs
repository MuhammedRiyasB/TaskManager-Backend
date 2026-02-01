using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Modules.Auth.Data;
using TaskManager.Modules.Auth.Jwt;
using TaskManager.Modules.Auth.Repositories;
using TaskManager.Modules.Auth.Security;
using TaskManager.Modules.Auth.Services;

namespace TaskManager.Modules.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddSingleton<IJwtTokenGenerator>(sp =>
            new JwtTokenGenerator(
                configuration["Jwt:Key"]!,
                configuration["Jwt:Issuer"]!,
                configuration["Jwt:Audience"]!));

        return services;
    }
}
