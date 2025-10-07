using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace dxt.Controller;

[ApiController]
[Route("[controller]")]
public class Player(Service.Player dtoPlayer,
                    Service.Names names) : ControllerBase
{
    [HttpGet]
    [Authorize()]
    [RequiredScope("Players.Read.All")]
    public async Task<ActionResult<List<Model.Player>>> Get() =>
        await dtoPlayer.GetAll();

    [HttpGet("{id}")]
    [Authorize]
    [RequiredScope("Players.Read.All")]
    public async Task<ActionResult<Model.Player>> Get(string id) {
        if(await dtoPlayer.Get(id) is Model.Player player)
            return player;
        return NotFound();
    }

    [HttpPost]
    [Authorize]
    [RequiredScope("Players.Write.All")]
    public async Task<IActionResult> Create(Model.Player player)
    {
        if(dtoPlayer.Contains(player.UniqueId))
            return Conflict("¡Esta cuenta ya esta registrada!");

        player.RecordNumber = names.Add(player.Name);
        await dtoPlayer.Add(player.ResolveSearchId());
        
        return CreatedAtAction(nameof(Get), new {id = player.UniqueId}, player);
    }

    [HttpPut("{id}")]
    [Authorize]
    [RequiredScope("Players.Update.Self")]
    public IActionResult Update(string id, Model.Player player)
    {
        if(id != player.UniqueId)
        {
            return BadRequest();
        }
        else if(dtoPlayer.Contains(player.UniqueId))
        {
            dtoPlayer.Update(player);
            return NoContent();
        }
        else return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if(dtoPlayer.GetByUniqueId(id) is Model.Player player)
        {
            try
            {
                await dtoPlayer.DeleteAsync(player);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex}");
            }
            return NoContent();
        }
        else return NotFound();
    }
}
