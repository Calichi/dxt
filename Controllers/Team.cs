using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dxt.Controller;

[ApiController]
[Route("[controller]")]
public class Team(Service.Team teams) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RegisterAsync(Model.Team team)
    {
        if ( await teams.ContainsAsync( team ) )
            return Conflict("¡Ya existe un equipo registrado con este nombre!");


        try
        {
            await teams.AddAsync( team );
            return CreatedAtAction(nameof(GetAsync), new {id = team.Id}, team);
        }
        catch ( Exception ex )
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Model.Team>> GetAsync(long id)
    {
        if ( await teams.GetAsync(id) is Model.Team team )
            return team;
            
        return NotFound();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Model.Team>>> GetAsync()
    {
        return await teams.GetAsync();
    }
}
