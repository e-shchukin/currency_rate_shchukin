using CurrencyRate.Dal.DbContext;
using CurrencyRate.Dal.DbEntities;
using CurrencyRate.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace CurrencyRate.Dal.Repositories;

public class CurrencyRateRepository : ICurrencyRateRepository
{
    private readonly ILogger<CurrencyRateRepository> _logger;
    
    
    public CurrencyRateRepository(ILogger<CurrencyRateRepository> logger)
    {
        _logger = logger;
    }
    
    public async Task SaveRatesToDb(List<(Currency, Rate)> ratesList)
    {
        using (var сurrencyRateContext = new CurrencyRateContext())
        {
            await ProcessCurrencyRate(сurrencyRateContext, ratesList);
            await сurrencyRateContext.SaveChangesAsync();
        }
    }

    public async Task SaveRatesToDb(List<List<(Currency, Rate)>> ratesList)
    {
        using (var сurrencyRateContext = new CurrencyRateContext())
        {
            foreach (var currencyRate in ratesList)
            {
                await ProcessCurrencyRate(сurrencyRateContext, currencyRate);
            }
            await сurrencyRateContext.SaveChangesAsync();
        }
    }

    private async Task ProcessCurrencyRate(CurrencyRateContext сurrencyRateContext, List<(Currency, Rate)> currencyRatePairs)
    {
        foreach (var currencyRatePair in currencyRatePairs)
        {
            var currency = currencyRatePair.Item1;
            var rate = currencyRatePair.Item2;
            try
            {
                var rateAlreadyExists =
                    сurrencyRateContext.Rates.Any(e =>
                        e.Date == rate.Date && e.CurrencyID == rate.CurrencyID);

                if (rateAlreadyExists) continue;

                var currencyAlreadyExists =
                    сurrencyRateContext.Currencies.Any(e => e.CurrencyID == currency.CurrencyID);

                if (!currencyAlreadyExists)
                {
                    сurrencyRateContext.Currencies.Add(currency);
                }

                сurrencyRateContext.Rates.Add(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}