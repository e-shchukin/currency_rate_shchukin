namespace CurrencyRate.Bll.Models.Dtos;

public class CurrencyDto
{
    public string CurrencyID { get; set; } = null!;
    
    public int NumCode { get; set; }
    
    public string CharCode { get; set; }
    
    public int Nominal { get; set; }
    
    public string Name { get; set; }
}


