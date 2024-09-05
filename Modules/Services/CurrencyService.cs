using currency_rate;
using CurrencyLoader.Modules.Api;
using CurrencyLoader.Modules.Data;


namespace CurrencyLoader
{
    public class CurrencyService
    {
        private CbrApiClient _cbrApiClient = new();
        private CurrencyRepository _currencyRepository = new();
        private XmlRatesParcer _xmlRatesParcer = new();
        
        
        public async Task FillRatesDailyAsync()
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            
            var xml = await _cbrApiClient.GetRatesForDateAsync(dateNow);
            var rates = _xmlRatesParcer.ParceXmlToRates(xml, dateNow);
            
            _currencyRepository.SaveRatesToDb(rates);
        }

        public async Task FillRatesMonthlyAsync()
        {
            var monthlyRatesList = new List<List<CurrencyRate>>();
            
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            var startDate = dateNow.AddMonths(-1);
            
            while (startDate <= dateNow)
            {
                var xml = await _cbrApiClient.GetRatesForDateAsync(startDate);
                var rates = _xmlRatesParcer.ParceXmlToRates(xml, startDate);
                monthlyRatesList.Add(rates);
                
                startDate = startDate.AddDays(1);
            }
            _currencyRepository.SaveRatesToDb(monthlyRatesList);
        }
    }
}