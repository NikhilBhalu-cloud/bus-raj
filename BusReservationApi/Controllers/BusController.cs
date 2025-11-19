using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusReservationApi.Data;
using BusReservationApi.DTOs;

namespace BusReservationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusController : ControllerBase
    {
        private readonly BusReservationContext _context;

        public BusController(BusReservationContext context)
        {
            _context = context;
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<RouteResult>>> SearchRoutes([FromBody] SearchRequest request)
        {
            var routes = await _context.Routes.ToListAsync();
            var results = new List<RouteResult>();

            foreach (var route in routes)
            {
                var stations = route.Stations.Split(',').Select(s => s.Trim()).ToList();
                
                // Check if both source and destination are in this route
                var sourceIndex = stations.IndexOf(request.Source);
                var destIndex = stations.IndexOf(request.Destination);

                // Only include routes where both stations exist and source comes before destination
                if (sourceIndex >= 0 && destIndex >= 0 && sourceIndex < destIndex)
                {
                    // Calculate available seats based on bookings that overlap with this segment
                    var overlappingBookings = await _context.Bookings
                        .Where(b => b.RouteId == route.Id)
                        .ToListAsync();

                    int bookedSeats = 0;
                    foreach (var booking in overlappingBookings)
                    {
                        var bookingSourceIndex = stations.IndexOf(booking.SourceStation);
                        var bookingDestIndex = stations.IndexOf(booking.DestinationStation);

                        // Check if booking segments overlap
                        // Bookings overlap if they share any part of the journey
                        if (!(bookingDestIndex <= sourceIndex || bookingSourceIndex >= destIndex))
                        {
                            bookedSeats += booking.SeatsBooked;
                        }
                    }

                    results.Add(new RouteResult
                    {
                        RouteId = route.Id,
                        RouteName = route.Name,
                        AvailableSeats = route.TotalSeats - bookedSeats,
                        TotalSeats = route.TotalSeats
                    });
                }
            }

            return Ok(results);
        }

        [HttpPost("book")]
        public async Task<ActionResult> BookSeats([FromBody] BookingRequest request)
        {
            var route = await _context.Routes.FindAsync(request.RouteId);
            if (route == null)
            {
                return NotFound("Route not found");
            }

            var stations = route.Stations.Split(',').Select(s => s.Trim()).ToList();
            var sourceIndex = stations.IndexOf(request.Source);
            var destIndex = stations.IndexOf(request.Destination);

            if (sourceIndex < 0 || destIndex < 0 || sourceIndex >= destIndex)
            {
                return BadRequest("Invalid source or destination");
            }

            // Check available seats for this segment
            var overlappingBookings = await _context.Bookings
                .Where(b => b.RouteId == request.RouteId)
                .ToListAsync();

            int bookedSeats = 0;
            foreach (var booking in overlappingBookings)
            {
                var bookingSourceIndex = stations.IndexOf(booking.SourceStation);
                var bookingDestIndex = stations.IndexOf(booking.DestinationStation);

                if (!(bookingDestIndex <= sourceIndex || bookingSourceIndex >= destIndex))
                {
                    bookedSeats += booking.SeatsBooked;
                }
            }

            int availableSeats = route.TotalSeats - bookedSeats;
            if (request.Seats > availableSeats)
            {
                return BadRequest($"Only {availableSeats} seats available");
            }

            // Create booking
            var newBooking = new Models.Booking
            {
                RouteId = request.RouteId,
                SourceStation = request.Source,
                DestinationStation = request.Destination,
                SeatsBooked = request.Seats,
                BookingDate = DateTime.UtcNow
            };

            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking successful", bookingId = newBooking.Id });
        }
    }
}
