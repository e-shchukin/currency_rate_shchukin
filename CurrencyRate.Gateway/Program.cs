using CurrencyRate.Bll.Extensions;
using CurrencyRate.Bll.Services;
using CurrencyRate.Bll.Services.Interfaces;
using CurrencyRate.Dal.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CurrencyRate.Gateway
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<ICurrencyService, CurrencyService>()
                .AddBllServices()
                .AddDalServices()
                .AddLogging(builder =>
                {
                    builder.AddConsole(options =>
                    {
                        options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
                    });
                })
                .BuildServiceProvider();
            
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var сurrencyService = serviceProvider.GetService<ICurrencyService>();
            
            logger.LogInformation($"Application started.");
            
            await сurrencyService.FillRatesDailyAsync();
            await сurrencyService.FillRatesMonthlyAsync();

            logger.LogInformation($"The application has completed its work.");
        }
    }
}