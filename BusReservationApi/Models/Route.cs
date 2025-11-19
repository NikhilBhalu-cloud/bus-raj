namespace BusReservationApi.Models
{
    public class BusRoute
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Stations { get; set; } = string.Empty; // Comma-separated station names
        public int TotalSeats { get; set; } = 60;
    }
}
