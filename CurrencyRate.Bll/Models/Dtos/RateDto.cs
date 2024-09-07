namespace CurrencyRate.Bll.Models.Dtos;

public class RateDto
{
    public string CurrencyID { get; set; } = null!;
    
    public decimal Value { get; set; }
    
    public double VunitRate { get; set; }
    
    public DateOnly Date { get; set; }
}