namespace CarRental.Models;

public class Booking
{
    public required string BookingNumber { get; set; }
    public required string RegistrationNumber { get; set; }
    public required string CustomerSSN { get; set; }
    public required VehicleType VehicleType { get; set; }
    public required DateTime StartDate { get; set; }
    public required int StartMileage { get; set; }
    public DateTime? EndDate { get; set; }
    public int? EndMileage { get; set; }
    public decimal? RentalRate { get; set; } // Assumption: we want to store this for future analytics
}
