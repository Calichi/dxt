using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace dxt.Controller;

[ApiController]
[Route("[controller]")]
public class Team(Service.Team teams) : ControllerBase
{
    [HttpPost]
    [Authorize]
    [RequiredScope("Players.Write.All")]
    public async Task<IActionResult> RegisterAsync(Model.Team team)
    {
        try
        {
            if ( await teams.ContainsAsync( team ) )
                return Conflict("¡Ya existe un equipo registrado con este nombre!");

            await teams.AddAsync( team );
            var result = CreatedAtAction(nameof(GetAsync), new {id = team.Id}, team);
            return result;
        }
        catch ( Exception ex )
        {
            return BadRequest($"HANDLED ERROR: {ex}");
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    [RequiredScope("Players.Read.All")]
    public async Task<ActionResult<Model.Team>> GetAsync(long id)
    {
        try
        {
            if ( await teams.GetAsync(id) is Model.Team team )
                return team;
                
            return NotFound();
        }
        catch ( Exception ex )
        {
            return BadRequest($"HANDLED ERROR: {ex}");
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Model.Team>>> GetAllAsync()
    {
        return await teams.GetAllAsync();
    }
}
