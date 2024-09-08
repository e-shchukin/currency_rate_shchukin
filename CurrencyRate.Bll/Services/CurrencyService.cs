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
using Microsoft.Extensions.Logging;

namespace CurrencyRate.Bll.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICbrApiClient _cbrApiClient;
        private readonly IXmlRatesParser _xmlRatesParser;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly ILogger<CurrencyService> _logger;
        
        public CurrencyService(
            ICbrApiClient cbrApiClient, 
            IXmlRatesParser xmlRatesParser, 
            ICurrencyRateRepository currencyRateRepository,
            ILogger<CurrencyService> logger)
        {
            _cbrApiClient = cbrApiClient;
            _xmlRatesParser = xmlRatesParser;
            _currencyRateRepository = currencyRateRepository;
            _logger = logger;
        }

        public async Task FillRatesDailyAsync()
        {
            _logger.LogInformation($"Daily currency rates update process started.");
            
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            var xml = await _cbrApiClient.GetRatesForDateAsync(dateNow);
            var dtoRates = _xmlRatesParser.ParseXmlToRates(xml, dateNow);

            var rates = MapDtoToEntities(dtoRates);
            await _currencyRateRepository.SaveRatesToDb(rates);
            
            _logger.LogInformation("Daily currency rates update process completed.");
        }

        public async Task FillRatesMonthlyAsync()
        {
            _logger.LogInformation($"Monthly currency rates update process started.");
            
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
            
            _logger.LogInformation("Monthly currency rates update process completed.");
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