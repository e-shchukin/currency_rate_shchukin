namespace CurrencyRate.Bll.Services.Interfaces;

public interface ICurrencyService
{
    public Task FillRatesDailyAsync();

    public Task FillRatesMonthlyAsync();
}