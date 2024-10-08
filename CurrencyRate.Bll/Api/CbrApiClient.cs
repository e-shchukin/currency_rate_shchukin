﻿using System.Text;
using CurrencyRate.Bll.Api.Interfaces;

namespace CurrencyRate.Bll.Api;

internal class CbrApiClient : ICbrApiClient
{
    private const string CbrCurrencyRatesUrl = "https://www.cbr.ru/scripts/XML_daily.asp";
    private readonly Encoding _encoding;

    public CbrApiClient()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        _encoding = Encoding.GetEncoding("windows-1251");
    }

    public async Task<string> GetRatesForDateAsync(DateOnly date)
    {
        var url = $"{CbrCurrencyRatesUrl}?date_req={date:dd/MM/yyyy}";
        
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseStream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(responseStream, _encoding);
        return await reader.ReadToEndAsync();
    }
}