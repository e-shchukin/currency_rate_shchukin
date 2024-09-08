namespace CurrencyRate.Bll.Api.Interfaces;

public interface ICbrApiClient
{
    public Task<string> GetRatesForDateAsync(DateOnly date);
}