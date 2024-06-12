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
    public IActionResult Register(Model.Team team)
    {
        try
        {
            if ( teams.Contains( team ) )
                return Conflict("¡Ya existe un equipo registrado con este nombre!");

            teams.Add( team );
            return CreatedAtAction(nameof(Get), new {id = team.Id}, team);
        }
        catch ( Exception ex )
        {
            return BadRequest($"HANDLED ERROR: {ex}");
        }
    }

    [HttpGet("{id:long}")]
    [ActionName(nameof(Get))]
    [Authorize]
    [RequiredScope("Players.Read.All")]
    public ActionResult<Model.Team> Get(long id)
    {
        try
        {
            if ( teams.Get(id) is Model.Team team )
                return team;
                
            return NotFound();
        }
        catch ( Exception ex )
        {
            return BadRequest($"HANDLED ERROR: {ex}");
        }
    }

    [HttpGet("{searchId}")]
    [Authorize]
    public ActionResult<ICollection<Model.Team>> Get(string searchId)
    {
        try
        {
            return teams.Get(searchId);
        }
        catch( Exception ex )
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
