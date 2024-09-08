using System.Globalization;
using System.Xml.Linq;
using CurrencyRate.Bll.Helpers.Interfaces;
using CurrencyRate.Bll.Models.Dtos;
using Microsoft.Extensions.Logging;

namespace CurrencyRate.Bll.Helpers;

internal class XmlRatesParser : IXmlRatesParser
{
    private readonly CultureInfo _cultureInfo = new("ru-RU");
    private readonly ILogger<XmlRatesParser> _logger;
    
    public XmlRatesParser(ILogger<XmlRatesParser> logger)
    {
        _logger = logger;
    }
    
    public List<(CurrencyDto, RateDto)> ParseXmlToRates(string xml, DateOnly currentDate)
    {
        var document = XDocument.Parse(xml);
        if (!ValidateXml(document, currentDate))
        {
            _logger.LogError("XML validation failed. Dates in the document and in the request differ.");
            return new List<(CurrencyDto, RateDto)>();
        }

        var date = DateOnly.ParseExact(document.Root.Attribute("Date").Value, "dd.MM.yyyy");

        return document.Descendants("Valute").Select(element =>
        {
            var currencyDto = new CurrencyDto
            {
                CurrencyID = element.Attribute("ID").Value,
                NumCode = ushort.Parse(element.Element("NumCode").Value),
                CharCode = element.Element("CharCode").Value,
                Nominal = int.Parse(element.Element("Nominal").Value),
                Name = element.Element("Name").Value,
            };
            var rateDto = new RateDto
            {
                CurrencyID = element.Attribute("ID").Value,
                Value = decimal.Parse(element.Element("Value").Value, _cultureInfo),
                VunitRate = double.Parse(element.Element("VunitRate").Value, _cultureInfo),
                Date = date,
            };
            
            return (currencyDto, rateDto);
        }).ToList();
    }

    //Функция для проверки, соответствует ли полученный отчет по дате от запрошенной даты
    private bool ValidateXml(XDocument document, DateOnly currentDate)
    {
        var date = DateOnly.ParseExact(document.Root.Attribute("Date").Value, "dd.MM.yyyy");
        return currentDate == date;
    }
}