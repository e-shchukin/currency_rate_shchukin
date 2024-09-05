using currency_rate;

namespace CurrencyLoader.Modules.Data;

public class CurrencyRepository
{
    public async Task SaveRatesToDb(List<CurrencyRate> ratesList)
    {
        using (var сurrencyRateContext = new CurrencyRateContext())
        {
            await ProcessCurrencyRate(сurrencyRateContext, ratesList);
            сurrencyRateContext.SaveChanges();
        }
    }

    public async Task SaveRatesToDb(List<List<CurrencyRate>> ratesList)
    {
        using (var сurrencyRateContext = new CurrencyRateContext())
        {
            foreach (var currencyRate in ratesList)
            {
                await ProcessCurrencyRate(сurrencyRateContext, currencyRate);
            }
            сurrencyRateContext.SaveChanges();
        }
    }

    private async Task ProcessCurrencyRate(CurrencyRateContext сurrencyRateContext,
        List<CurrencyRate> currencyRates)
    {
        foreach (var currencyRate in currencyRates)
        {
            try
            {
                var rateAlreadyExists =
                    сurrencyRateContext.Rates.Any(e =>
                        e.Date == currencyRate.Date && e.CurrencyID == currencyRate.ID);

                if (rateAlreadyExists) continue;

                var currencyAlreadyExists =
                    сurrencyRateContext.Currencies.Any(e => e.CurrencyID == currencyRate.ID);

                if (!currencyAlreadyExists)
                {
                    сurrencyRateContext.Currencies.Add(new Currency()
                    {
                        CurrencyID = currencyRate.ID,
                        NumCode = currencyRate.NumCode,
                        CharCode = currencyRate.CharCode,
                        Nominal = currencyRate.Nominal,
                        Name = currencyRate.Name
                    });
                }

                сurrencyRateContext.Rates.Add(new Rate()
                {
                    CurrencyID = currencyRate.ID,
                    Value = currencyRate.Value,
                    VunitRate = currencyRate.VunitRate,
                    Date = currencyRate.Date
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}