using AirlinesAPI.DTOs;
using AirlinesAPI.Entities;
using AirlinesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirlinesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirlinesController : ControllerBase
    {
        private readonly AirlinesService _airlinesService;

        public AirlinesController(AirlinesService airlinesService) =>
            _airlinesService = airlinesService;

        [HttpGet]
        public Task<List<Airline>> GetAll() =>
            _airlinesService.GetAllWithFleetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Airline>> Get(string id)
        {
            var a = await _airlinesService.GetWithFleetAsync(id);
            return a == null ? NotFound() : a;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAirlineDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var airline = new Airline
            {
                Name = dto.Name,
                IATA = dto.IATA,
                ICAO = dto.ICAO
            };

            try
            {
                await _airlinesService.CreateAsync(airline);
                return CreatedAtAction(nameof(Get), new { id = airline.Id }, airline);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, UpdateAirlineDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _airlinesService.GetAsync(id); // throws if not found
                existing.Name = dto.Name;
                existing.IATA = dto.IATA;
                existing.ICAO = dto.ICAO;
                await _airlinesService.UpdateAsync(id, existing);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var exists = await _airlinesService.GetAsync(id);
            if (exists == null) return NotFound();
            await _airlinesService.RemoveAsync(id);
            return NoContent();
        }

        [HttpGet("{id:length(24)}/fleet")]
        public Task<List<Aircraft>> GetFleet(string id) =>
            _airlinesService.GetFleetAsync(id);

        [HttpPut("{airlineId:length(24)}/fleet/{aircraftId:length(24)}")]
        public async Task<IActionResult> AssignToFleet(string airlineId, string aircraftId)
        {
            try
            {
                await _airlinesService.AssignAircraftToFleetAsync(airlineId, aircraftId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{airlineId:length(24)}/fleet/{aircraftId:length(24)}")]
        public async Task<IActionResult> RemoveFromFleet(string airlineId, string aircraftId)
        {
            await _airlinesService.RemoveAircraftFromFleetAsync(airlineId, aircraftId);
            return NoContent();
        }
    }
}
