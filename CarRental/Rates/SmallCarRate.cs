namespace CarRental.Rates;

public class SmallCarRate : IRateCalculator
{
    public decimal CalculateRate(RateInput rateInput)
    {
        return rateInput.BaseDayRental * rateInput.NumberOfDays;
    }
}
