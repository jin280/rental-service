namespace CarRental.Rates;

public interface IRateCalculator
{
    decimal CalculateRate(RateInput rateInput);
}
