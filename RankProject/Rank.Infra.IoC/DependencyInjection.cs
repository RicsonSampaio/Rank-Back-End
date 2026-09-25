using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rank.Application.App;
using Rank.Core.Auth;
using Rank.Core.Repository;
using Rank.Core.Service;
using Rank.Infra.Data.MySql.Common;

namespace Rank.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddRankServices(this IServiceCollection services, IConfiguration configuration)
    {
        DBDapperComponent.Configure(configuration);
        services.AddSingleton<UserRepository>();
        services.AddSingleton<TokenGenerator>();
        services.AddScoped<AccountService>();
        services.AddScoped<AccountApp>();
        services.AddScoped<UserService>();
        services.AddScoped<UserApp>();
        return services;
    }
}
