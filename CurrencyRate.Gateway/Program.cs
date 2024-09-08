using CurrencyRate.Bll.Extensions;
using CurrencyRate.Bll.Services;
using CurrencyRate.Bll.Services.Interfaces;
using CurrencyRate.Dal.DbContext;
using CurrencyRate.Dal.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CurrencyRate.Gateway;

internal class Program
{
    private static async Task Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<ICurrencyService, CurrencyService>()
            .AddBllServices()
            .AddDalServices()
            .AddLogging(builder =>
            {
                builder.AddConsole(options => { options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] "; });
            })
            .BuildServiceProvider();

        var logger = serviceProvider.GetService<ILogger<Program>>();
        var currencyService = serviceProvider.GetService<ICurrencyService>();

        logger!.LogInformation("Application started");

        var context = serviceProvider.GetService<CurrencyRateContext>();
        await context!.Database.EnsureCreatedAsync();
        
        await currencyService!.FillRatesDailyAsync();
        await currencyService.FillRatesMonthlyAsync();

        logger!.LogInformation("The application has completed its work");
    }
}