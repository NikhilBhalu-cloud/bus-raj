namespace BusReservationApi.DTOs
{
    public class RouteResult
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
        public int TotalSeats { get; set; }
    }
}
