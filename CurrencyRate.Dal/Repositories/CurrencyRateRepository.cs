using CurrencyRate.Dal.DbContext;
using CurrencyRate.Dal.DbEntities;
using CurrencyRate.Dal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRate.Dal.Repositories;

public class CurrencyRateRepository : ICurrencyRateRepository
{
    private readonly ILogger<CurrencyRateRepository> _logger;
    private readonly CurrencyRateContext _context;

    public CurrencyRateRepository(
        CurrencyRateContext context, 
        ILogger<CurrencyRateRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SaveRatesToDb(List<(Currency, Rate)> ratesList)
    {
        await ProcessCurrencyRate(ratesList);
        await _context.SaveChangesAsync();
    }

    public async Task SaveRatesToDb(List<List<(Currency, Rate)>> ratesList)
    {
        foreach (var currencyRate in ratesList)
        {
            await ProcessCurrencyRate(currencyRate);
        }
        
        await _context.SaveChangesAsync();
    }

    private async Task ProcessCurrencyRate(List<(Currency Currency, Rate Rate)> currencyRatePairs)
    {
        foreach (var currencyRatePair in currencyRatePairs)
        {
            var currency = currencyRatePair.Currency;
            var rate = currencyRatePair.Rate;
            
            try
            {
                var rateAlreadyExists = await _context.Rates
                    .AnyAsync(e => e.Date == rate.Date && e.CurrencyID == rate.CurrencyID);

                if (rateAlreadyExists) continue;

                var currencyAlreadyExists = await _context.Currencies
                    .AnyAsync(e => e.CurrencyID == currency.CurrencyID);

                if (!currencyAlreadyExists)
                {
                    _context.Currencies.Add(currency);
                    await _context.SaveChangesAsync();
                }

                _context.Rates.Add(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}