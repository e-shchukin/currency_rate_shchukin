using CurrencyRate.Dal.DbEntities;

namespace CurrencyRate.Dal.Repositories.Interfaces;

public interface ICurrencyRateRepository
{
    public Task SaveRatesToDb(List<(Currency, Rate)> ratesList);

    public Task SaveRatesToDb(List<List<(Currency, Rate)>> ratesList);
}