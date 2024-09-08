using System.Reflection;
using CurrencyRate.Bll.Api;
using CurrencyRate.Bll.Api.Interfaces;
using CurrencyRate.Bll.Helpers;
using CurrencyRate.Bll.Helpers.Interfaces;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRate.Bll.Extensions;

public static class ServiceCollectionExtensionBll
{
    public static IServiceCollection AddBllServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICbrApiClient, CbrApiClient>();
        serviceCollection.AddScoped<IXmlRatesParser, XmlRatesParser>();
        serviceCollection.AddMapping();

        return serviceCollection;
    }

    private static void AddMapping(this IServiceCollection serviceCollection)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        serviceCollection.AddSingleton(config);
        serviceCollection.AddMapster();
    }
}