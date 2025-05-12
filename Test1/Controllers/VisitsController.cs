using Microsoft.AspNetCore.Mvc;
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
    
    
}