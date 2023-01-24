using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SailpointBackend.Models;

namespace SailpointBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CityContext _context;

        public CitiesController(CityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities([FromQuery] string searchTerm, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            if (_context.Cities == null || string.IsNullOrEmpty(searchTerm))
            {
                return NotFound();
            }

            var valuesToSkip = pageIndex * pageSize;

            var results = await _context.Cities.Where(x => x.Name.Contains(searchTerm))
                .OrderBy(c => c.Name).Skip(valuesToSkip).Take(pageSize).ToListAsync();

            return Ok(results);
        }
    }
}