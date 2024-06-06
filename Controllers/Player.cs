using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace dxt.Controller;

[ApiController]
[Route("[controller]")]
public class Player(Service.Player dtoPlayer) : ControllerBase
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
    public async Task<IActionResult> Create(Model.Player model)
    {
        if(dtoPlayer.Contains(model.UniqueId))
            return Conflict("¡Esta cuenta ya esta registrada!");

        await dtoPlayer.Add(model);
        return CreatedAtAction(nameof(Get), new {id = model.UniqueId}, model);
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
    public IActionResult Delete(string id)
    {
        if(dtoPlayer.GetByUniqueId(id) is Model.Player player)
        {
            dtoPlayer.Delete(player);
            return NoContent();
        }
        else return NotFound();
    }
}
