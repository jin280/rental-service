namespace CarRental.Rates;

public class RateInput
{
    public required decimal BaseDayRental { get; set; }
    public required int NumberOfDays { get; set; }
    public decimal BaseKmPrice { get; set; }
    public decimal NumberOfKm { get; set; }
}
