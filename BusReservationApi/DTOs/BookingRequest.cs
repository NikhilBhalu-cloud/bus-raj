namespace BusReservationApi.DTOs
{
    public class BookingRequest
    {
        public int RouteId { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int Seats { get; set; }
    }
}
