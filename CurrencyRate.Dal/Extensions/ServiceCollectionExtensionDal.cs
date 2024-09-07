using CurrencyRate.Dal.Repositories;
using CurrencyRate.Dal.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRate.Dal.Extensions;

public static class ServiceCollectionExtensionDal
{
    public static IServiceCollection AddDalServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
        return serviceCollection;
    }
}