using CarRental.Rates;

namespace CarRental.Tests;

public class RateCalculatorTests
{
    [Fact]
    public void SmallCarRate_ShouldReturnCorrectPrice_WhenRentedMultipleDays()
    {
        // Arrange
        var rateCalculator = new SmallCarRate();
        var rateInput = new RateInput
        {
            BaseDayRental = 50m,
            NumberOfDays = 3
        };

        // Act
        decimal result = rateCalculator.CalculateRate(rateInput);

        // Assert
        Assert.Equal(150m, result);
    }

    [Fact]
    public void CombiRate_ShouldReturnCorrectPrice_WhenRentedMultipleDaysWithKm()
    {
        // Arrange
        var rateCalculator = new CombiRate();
        var rateInput = new RateInput
        {
            BaseDayRental = 50m,
            NumberOfDays = 3,
            BaseKmPrice = 0.5m,
            NumberOfKm = 100m
        };

        // Act
        decimal result = rateCalculator.CalculateRate(rateInput);

        // Assert
        Assert.Equal(245m, result);
    }

    [Fact]
    public void TruckRate_ShouldReturnCorrectPrice_WhenRentedMultipleDaysWithKm()
    {
        // Arrange
        var rateCalculator = new TruckRate();
        var rateInput = new RateInput
        {
            BaseDayRental = 80m,
            NumberOfDays = 2,
            BaseKmPrice = 0.7m,
            NumberOfKm = 150m
        };

        // Act
        decimal result = rateCalculator.CalculateRate(rateInput);

        // Assert
        Assert.Equal(397.5m, result);
    }
}
