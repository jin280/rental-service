using CarRental.Models;
using CarRental.Rates;

namespace CarRental.Services;

public class RentalBookingService
{
    private readonly Dictionary<string, Booking> _bookings = new();
    private readonly Dictionary<VehicleType, IRateCalculator> _rateCalculators;

    public RentalBookingService(Dictionary<VehicleType, IRateCalculator> rateCalculators)
    {
        _rateCalculators = rateCalculators ?? throw new ArgumentNullException(nameof(rateCalculators));
    }


    public void RegisterPickUp(string bookingNumber, string registrationNumber, string customerSSN, VehicleType vehicleType, DateTime startDate, int startMileage)
    {
        if (string.IsNullOrWhiteSpace(bookingNumber))
            throw new ArgumentException("Booking number cannot be empty", nameof(bookingNumber));
        if (string.IsNullOrWhiteSpace(registrationNumber))
            throw new ArgumentException("Registration number cannot be empty", nameof(registrationNumber));
        if (string.IsNullOrWhiteSpace(customerSSN))
            throw new ArgumentException("Customer SSN cannot be empty", nameof(customerSSN));
        if (startMileage < 0)
            throw new ArgumentException("Start mileage cannot be negative", nameof(startMileage));
        if (_bookings.ContainsKey(bookingNumber))
            throw new InvalidOperationException($"Booking '{bookingNumber}' already exists");

        var booking = new Booking
        {
            BookingNumber = bookingNumber,
            RegistrationNumber = registrationNumber,
            CustomerSSN = customerSSN,
            VehicleType = vehicleType,
            StartDate = startDate,
            StartMileage = startMileage
        };
        _bookings[bookingNumber] = booking;
    }

    // Assumption: baseDayRental and baseKmPrice are provided as parameters
    public decimal RegisterReturn(string bookingNumber, DateTime endDate, int endMileage, decimal baseDayRental, decimal baseKmPrice)
    {
        if (!_bookings.TryGetValue(bookingNumber, out var booking))
            throw new KeyNotFoundException($"Booking '{bookingNumber}' not found");

        if (booking.EndDate != null)
            throw new InvalidOperationException("Return already registered for this booking");

        if (endDate < booking.StartDate)
            throw new ArgumentException("End date cannot be before start date", nameof(endDate));

        // Assumption: number of days is always rounded up to the nearest whole day,
        // and if the vehicle is returned on the same day, it counts as 1 day rental
        double numberOfDaysFractional = (endDate - booking.StartDate).TotalDays;
        int numberOfDays = Math.Max(1, (int)Math.Ceiling(numberOfDaysFractional));

        decimal numberOfKm = endMileage - booking.StartMileage;
        if (numberOfKm < 0)
            throw new ArgumentException("Return mileage cannot be less than pickup mileage", nameof(endMileage));

        if (!_rateCalculators.TryGetValue(booking.VehicleType, out var rateCalculator))
            throw new InvalidOperationException($"No rate calculator found for vehicle type '{booking.VehicleType}'");

        var rateInput = new RateInput
        {
            BaseDayRental = baseDayRental,
            NumberOfDays = numberOfDays,
            BaseKmPrice = baseKmPrice,
            NumberOfKm = numberOfKm
        };

        decimal rentalRate = rateCalculator.CalculateRate(rateInput);
        booking.RentalRate = rentalRate;
        booking.EndDate = endDate;
        booking.EndMileage = endMileage;
        return rentalRate;
    }
}
