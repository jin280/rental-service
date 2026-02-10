namespace CarRental.Rates;

public class CombiRate : IRateCalculator
{
    public decimal CalculateRate(RateInput rateInput)
    {
        return rateInput.BaseDayRental * rateInput.NumberOfDays * 1.3m + rateInput.BaseKmPrice * rateInput.NumberOfKm;
    }
}
