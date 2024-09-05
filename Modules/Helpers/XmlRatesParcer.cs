using currency_rate;
using System.Xml.Linq;
using System.Globalization;

namespace CurrencyLoader;

public class XmlRatesParcer
{
    
    private readonly CultureInfo _cultureInfo = new("ru-RU");
    
    public List<CurrencyRate> ParceXmlToRates(string xml, DateOnly currentDate)
    {
        var document = XDocument.Parse(xml);
        var rates = new List<CurrencyRate>();

        if (!ValidateXml(document, currentDate))
        {
            Console.WriteLine("The dates in the document and in the request differ.");
            return rates;
        }
        
        var date = DateOnly.ParseExact(document.Root.Attribute("Date").Value, "dd.MM.yyyy");
        
        foreach (var element in document.Descendants("Valute"))
        {
            var rate = new CurrencyRate
            {
                ID = element.Attribute("ID").Value,
                NumCode = ushort.Parse(element.Element("NumCode").Value),
                CharCode = element.Element("CharCode").Value,
                Nominal = int.Parse(element.Element("Nominal").Value),
                Name = element.Element("Name").Value,
                Value = decimal.Parse(element.Element("Value").Value, _cultureInfo),
                VunitRate = double.Parse(element.Element("VunitRate").Value, _cultureInfo),
                Date = date
            };
            rates.Add(rate);
        }
        return rates;
    }
    
    //Функция для проверки, соответствует ли полученный отчет по дате от запрошенной даты
    private bool ValidateXml(XDocument document, DateOnly currentDate)
    {
        var date = DateOnly.ParseExact(document.Root.Attribute("Date").Value, "dd.MM.yyyy");

        if (currentDate != date)
        {
            return false;
        }

        return true;
    }
}