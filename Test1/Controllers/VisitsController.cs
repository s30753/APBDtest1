using Microsoft.AspNetCore.Mvc;
using Test1.Models;
using Test1.Services;

namespace Test1.Controllers;

[ApiController]
[Route("api/visits")]
public class VisitsController : ControllerBase
{
    private readonly IVisitsService _visitsService;

    public VisitsController(IVisitsService visitsService)
    {
        _visitsService = visitsService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVisitById(int id)
    {
        var result = await _visitsService.GetVisitByIdAsync(id);
        if (result == null) return NotFound("Visit with a given ID not found");
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddVisit([FromBody] Visit visit)
    {
        var result = await _visitsService.AddVisitAsync(visit);
        if (result == -1) return Conflict("Visit with a given visit already exists");
        if (result == -2) return NotFound("Client with a given visit doesn't exists");
        if (result == -3) return NotFound("Mechanic with a given licence number doesn't exists");
        return Ok(result);
    }
}