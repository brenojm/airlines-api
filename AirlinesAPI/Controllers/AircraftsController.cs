using AirlinesAPI.DTOs;
using AirlinesAPI.Entities;
using AirlinesAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AircraftsController : ControllerBase
{
    private readonly AircraftsService _aircraftsService;
    private readonly AirlinesService _airlinesService;

    public AircraftsController(
        AircraftsService aircraftsService,
        AirlinesService airlinesService)
    {
        _aircraftsService = aircraftsService;
        _airlinesService = airlinesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AircraftDTO>>> GetAll()
    {
        var list = await _aircraftsService.GetAsync();
        var dtos = new List<AircraftDTO>();

        foreach (var a in list)
        {
            string? name = null;

            if (!string.IsNullOrEmpty(a.AirlineId))
            {
                try
                {
                    name = (await _airlinesService.GetAsync(a.AirlineId)).Name;
                }
                catch (KeyNotFoundException)
                {
                    // ignore missing airline
                }
            }

            dtos.Add(new AircraftDTO
            {
                Id = a.Id!,
                MSN = a.MSN,
                Type = a.Type,
                Registration = a.Registration,
                AirlineName = name
            });
        }

        return Ok(dtos);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var a = await _aircraftsService.GetAsync(id);
            string? name = null;

            if (!string.IsNullOrEmpty(a.AirlineId))
                name = (await _airlinesService.GetAsync(a.AirlineId)).Name;

            var dto = new AircraftDTO
            {
                Id = a.Id!,
                MSN = a.MSN,
                Type = a.Type,
                Registration = a.Registration,
                AirlineName = name
            };

            return Ok(dto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAircraftDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var aircraft = new Aircraft
        {
            MSN = dto.MSN,
            Type = dto.Type,
            Registration = dto.Registration,
        };

        try
        {
            await _aircraftsService.CreateAsync(aircraft);
            return CreatedAtAction(nameof(Get), new { id = aircraft.Id }, aircraft);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UpdateAircraftDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var existing = await _aircraftsService.GetAsync(id);
            existing.MSN = dto.MSN;
            existing.Type = dto.Type;
            existing.Registration = dto.Registration;
            await _aircraftsService.UpdateAsync(id, existing);
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
        try
        {
            await _aircraftsService.RemoveAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
