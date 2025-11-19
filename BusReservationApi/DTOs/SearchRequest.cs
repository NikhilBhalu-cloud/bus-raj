namespace BusReservationApi.DTOs
{
    public class SearchRequest
    {
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
    }
}
