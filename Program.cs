using currency_rate;

namespace CurrencyLoader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Time start: {DateTime.Now}");
            
            using (var _context = new CurrencyRateContext())
            {
                _context.Database.EnsureCreated();
            }
            
            var service = new CurrencyService();

            await service.FillRatesDailyAsync();

            await service.FillRatesMonthlyAsync();

            Console.WriteLine($"Time end: {DateTime.Now}");
        }
    }
}