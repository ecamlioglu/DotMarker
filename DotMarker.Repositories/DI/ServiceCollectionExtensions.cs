using DotMarker.Repositories.Interfaces;
using DotMarker.Repositories.Operations;
using DotMarker.Repositories.UOW;
using Microsoft.Extensions.DependencyInjection;

namespace DotMarker.Repositories.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}