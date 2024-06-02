using System.Collections.Generic;
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
    public IActionResult RegisterAsync(Model.Team team)
    {
        return Ok("Entro");
        // if ( await teams.ContainsAsync( team ) )
        //     return Conflict("¡Ya existe un equipo registrado con este nombre!");

        // await teams.AddAsync( team );
        // return CreatedAtAction(nameof(GetAsync), new {id = team.Id}, team);
    }

    [HttpGet("{id}")]
    [Authorize]
    [RequiredScope("Players.Read.All")]
    public async Task<ActionResult<Model.Team>> GetAsync(long id)
    {
        if ( await teams.GetAsync(id) is Model.Team team )
            return team;
            
        return NotFound();
    }

    [HttpGet()]
    public async Task<ActionResult<List<Model.Team>>> GetAsync()
    {
        return Ok();
        //return Ok(await teams.GetAsync());
    }
}
