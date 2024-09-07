using CurrencyRate.Bll.Extensions;
using CurrencyRate.Bll.Services;
using CurrencyRate.Bll.Services.Interfaces;
using CurrencyRate.Dal.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRate.Gateway
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Time start: {DateTime.Now}");
            
            var serviceProvider = new ServiceCollection()
                .AddScoped<ICurrencyService, CurrencyService>()
                .AddBllServices()
                .AddDalServices()
                .BuildServiceProvider();
            
            var сurrencyService = serviceProvider.GetService<ICurrencyService>();
            await сurrencyService.FillRatesDailyAsync();
            await сurrencyService.FillRatesMonthlyAsync();

            Console.WriteLine($"Time end: {DateTime.Now}");
        }
    }
}