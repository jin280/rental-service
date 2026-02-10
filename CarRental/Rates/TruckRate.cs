namespace CarRental.Rates;

public class TruckRate : IRateCalculator
{
    public decimal CalculateRate(RateInput rateInput)
    {
        return rateInput.BaseDayRental * rateInput.NumberOfDays * 1.5m + rateInput.BaseKmPrice * rateInput.NumberOfKm * 1.5m;
    }
}
