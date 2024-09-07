using CurrencyRate.Dal.DbContext;
using CurrencyRate.Dal.DbEntities;
using CurrencyRate.Dal.Repositories.Interfaces;

namespace CurrencyRate.Dal.Repositories;

public class CurrencyRateRepository : ICurrencyRateRepository
{
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
                Console.WriteLine(ex.Message);
            }
        }
    }
}