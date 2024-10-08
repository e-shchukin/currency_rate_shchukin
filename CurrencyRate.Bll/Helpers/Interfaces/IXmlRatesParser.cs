﻿using CurrencyRate.Bll.Models.Dtos;

namespace CurrencyRate.Bll.Helpers.Interfaces;

public interface IXmlRatesParser
{
    public List<(CurrencyDto, RateDto)> ParseXmlToRates(string xml, DateOnly currentDate);
}