namespace BusReservationApi.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string SourceStation { get; set; } = string.Empty;
        public string DestinationStation { get; set; } = string.Empty;
        public int SeatsBooked { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
