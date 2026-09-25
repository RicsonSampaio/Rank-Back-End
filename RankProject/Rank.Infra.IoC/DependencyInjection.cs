using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rank.Application.App;
using Rank.Core.Repository;
using Rank.Core.Service;
using Rank.Infra.Data.MySql.Repository;

namespace Rank.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddRankServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository>(_ =>
        {
            var connectionString = configuration.GetConnectionString("Rank");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Configure ConnectionStrings:Rank.");
            return new UserRepository(connectionString);
        });
        services.AddScoped<AccountService>();
        services.AddScoped<AccountApp>();
        services.AddScoped<UserService>();
        services.AddScoped<UserApp>();
        return services;
    }
}
