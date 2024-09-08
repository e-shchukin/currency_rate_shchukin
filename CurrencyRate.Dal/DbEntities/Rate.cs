namespace CurrencyRate.Dal.DbEntities;

public class Rate
{
    public string CurrencyID { get; set; } = null!;

    public decimal Value { get; set; }

    public double VunitRate { get; set; }

    public DateOnly Date { get; set; }

    //[ForeignKey(nameof(CurrencyID))]
    //public Currency Currency { get; set; }
}