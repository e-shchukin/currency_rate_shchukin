using CurrencyRate.Bll.Api;
using CurrencyRate.Bll.Api.Interfaces;
using CurrencyRate.Bll.Helpers;
using CurrencyRate.Bll.Helpers.Interfaces;
using CurrencyRate.Dal.Repositories;
using CurrencyRate.Bll.Models;
using CurrencyRate.Bll.Models.Dtos;
using CurrencyRate.Bll.Services.Interfaces;
using CurrencyRate.Dal.DbEntities;
using CurrencyRate.Dal.Repositories.Interfaces;
using Mapster;

namespace CurrencyRate.Bll.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICbrApiClient _cbrApiClient;
        private readonly IXmlRatesParser _xmlRatesParser;
        private readonly ICurrencyRateRepository _currencyRateRepository;

        public CurrencyService(
            ICbrApiClient cbrApiClient, 
            IXmlRatesParser xmlRatesParser, 
            ICurrencyRateRepository currencyRateRepository)
        {
            _cbrApiClient = cbrApiClient;
            _xmlRatesParser = xmlRatesParser;
            _currencyRateRepository = currencyRateRepository;
        }

        public async Task FillRatesDailyAsync()
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            var xml = await _cbrApiClient.GetRatesForDateAsync(dateNow);
            var dtoRates = _xmlRatesParser.ParseXmlToRates(xml, dateNow);

            var rates = MapDtoToEntities(dtoRates);
            await _currencyRateRepository.SaveRatesToDb(rates);
        }

        public async Task FillRatesMonthlyAsync()
        {
            List<List<(Currency, Rate)>> monthlyRatesList = new();
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            var startDate = dateNow.AddMonths(-1);

            while (startDate <= dateNow)
            {
                var xml = await _cbrApiClient.GetRatesForDateAsync(startDate);
                var dtoRates = _xmlRatesParser.ParseXmlToRates(xml, startDate);
                var rates = MapDtoToEntities(dtoRates);
                monthlyRatesList.Add(rates);

                startDate = startDate.AddDays(1);
            }
            
            await _currencyRateRepository.SaveRatesToDb(monthlyRatesList);
        }

        private List<(Currency, Rate)> MapDtoToEntities(List<(CurrencyDto, RateDto)> rates)
        {
            return rates.Select(x =>
            {
                var currencyEntity = x.Item1.Adapt<Currency>();
                var rateEntity = x.Item2.Adapt<Rate>();

                return (currencyEntity, rateEntity);
            }).ToList(); 
        }
    }
}