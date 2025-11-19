using Microsoft.AspNetCore.Mvc;
using BusReservationApi.Data;
using BusReservationApi.Models;

namespace BusReservationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationsController : ControllerBase
    {
        private readonly BusReservationContext _context;

        public StationsController(BusReservationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Station>> GetStations()
        {
            return _context.Stations.ToList();
        }
    }
}
