using CarRental.Services;
using CarRental.Models;
using CarRental.Rates;

namespace CarRental.Tests;

public class RentalBookingServiceTests
{
    private readonly RentalBookingService _service;

    public RentalBookingServiceTests()
    {
        _service = new RentalBookingService(new Dictionary<VehicleType, IRateCalculator>
        {
            { VehicleType.SmallCar, new SmallCarRate() },
            { VehicleType.Combi, new CombiRate() },
            { VehicleType.Truck, new TruckRate() }
        });
    }

    [Fact]
    public void RegisterReturn_ShouldReturnCorrectPrice_WhenSmallCarReturned()
    {
        // Arrange
        _service.RegisterPickUp("B001", "ABC123", "12345678-1234", VehicleType.SmallCar,
            new DateTime(2026, 1, 1), 10000);

        // Act
        decimal result = _service.RegisterReturn("B001",
            new DateTime(2026, 1, 5), 10200, 50m, 0.5m);

        // Assert
        Assert.Equal(200m, result);
    }

    [Fact]
    public void RegisterReturn_ShouldReturnCorrectPrice_WhenCombiReturned()
    {
        // Arrange
        _service.RegisterPickUp("B001", "ABC123", "12345678-1234", VehicleType.Combi,
            new DateTime(2026, 1, 1), 10000);

        // Act
        decimal result = _service.RegisterReturn("B001",
            new DateTime(2026, 1, 5), 10200, 50m, 0.5m);

        // Assert
        Assert.Equal(360m, result);
    }

    [Fact]
    public void RegisterReturn_ShouldReturnCorrectPrice_WhenTruckReturned()
    {
        // Arrange
        _service.RegisterPickUp("B001", "ABC123", "12345678-1234", VehicleType.Truck,
            new DateTime(2026, 1, 1), 10000);

        // Act
        decimal result = _service.RegisterReturn("B001",
            new DateTime(2026, 1, 5), 10200, 50m, 0.5m);

        // Assert
        Assert.Equal(450m, result);
    }

    [Fact]
    public void RegisterReturn_ShouldChargeOneDay_WhenReturnedSameDay()
    {
        // Arrange
        _service.RegisterPickUp("B001", "ABC123", "12345678-1234", VehicleType.SmallCar,
            new DateTime(2026, 1, 1), 10000);

        // Act
        decimal result = _service.RegisterReturn("B001",
            new DateTime(2026, 1, 1), 10050, 50m, 0.5m);

        // Assert
        Assert.Equal(50m, result);
    }

    [Fact]
    public void RegisterReturn_ShouldRoundUpToNextDay_WhenReturnedPartialDay()
    {
        // Arrange
        _service.RegisterPickUp("B001", "ABC123", "12345678-1234", VehicleType.SmallCar,
            new DateTime(2026, 1, 1, 9, 0, 0), 10000);

        // Act
        decimal result = _service.RegisterReturn("B001",
            new DateTime(2026, 1, 3, 14, 0, 0), 10100, 50m, 0.5m);

        // Assert
        Assert.Equal(150m, result);
    }

    [Fact]
    public void RegisterReturn_ShouldThrowKeyNotFoundException_WhenBookingNotFound()
    {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() =>
            _service.RegisterReturn("NONEXISTENT", new DateTime(2026, 1, 5), 10200, 50m, 0.5m));
    }

    [Fact]
    public void RegisterReturn_ShouldThrowInvalidOperationException_WhenAlreadyReturned()
    {
        // Arrange
        _service.RegisterPickUp("B001", "ABC123", "12345678-1234", VehicleType.SmallCar,
            new DateTime(2026, 1, 1), 10000);
        _service.RegisterReturn("B001", new DateTime(2026, 1, 5), 10200, 50m, 0.5m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            _service.RegisterReturn("B001", new DateTime(2026, 1, 6), 10300, 50m, 0.5m));
    }

    [Fact]
    public void RegisterReturn_ShouldThrowArgumentException_WhenEndDateBeforeStartDate()
    {
        // Arrange
        _service.RegisterPickUp("B001", "ABC123", "12345678-1234", VehicleType.SmallCar,
            new DateTime(2026, 1, 5), 10000);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _service.RegisterReturn("B001", new DateTime(2026, 1, 3), 10200, 50m, 0.5m));
    }

    [Fact]
    public void RegisterReturn_ShouldThrowArgumentException_WhenNegativeKm()
    {
        // Arrange
        _service.RegisterPickUp("B001", "ABC123", "12345678-1234", VehicleType.SmallCar,
            new DateTime(2026, 1, 1), 10000);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _service.RegisterReturn("B001", new DateTime(2026, 1, 5), 9000, 50m, 0.5m));
    }

    [Fact]
    public void RegisterPickUp_ShouldThrowInvalidOperationException_WhenDuplicateBookingNumber()
    {
        // Arrange
        _service.RegisterPickUp("B001", "ABC123", "12345678-1234", VehicleType.SmallCar,
            new DateTime(2026, 1, 1), 10000);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            _service.RegisterPickUp("B001", "ABC123", "12345678-1234", VehicleType.Combi,
                new DateTime(2026, 1, 2), 5000));
    }

    [Fact]
    public void RegisterPickUp_ShouldThrowArgumentException_WhenBookingNumberEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _service.RegisterPickUp("", "ABC123", "12345678-1234", VehicleType.SmallCar,
                new DateTime(2026, 1, 1), 10000));
    }
}
