namespace CurrencyRate.Dal.DbEntities;

public class Currency
{
    public string CurrencyID { get; set; } = null!;
    public int NumCode { get; set; }
    public string CharCode { get; set; }
    public int Nominal { get; set; }
    
    public string Name { get; set; }

    //TODO
    //public List<Rate> Rates { get; set; }
}