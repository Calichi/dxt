using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace dxt.Controllers;

[ApiController]
[Route("[controller]")]
public class Player(Services.Player sport) : ControllerBase
{
    string? AccountId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    [HttpGet]
    [Authorize()]
    [RequiredScope("Players.Read.All")]
    public async Task<ActionResult<List<Model.Player>>> Get() =>
        await sport.GetAll();

    [HttpGet("{id}")]
    [Authorize]
    [RequiredScope("Players.Read.All")]
    public async Task<ActionResult<Model.Player>> Get(long id) {
        if(await sport.Get(id) is Model.Player player)
            return player;
        return NotFound();
    }

    [HttpPost]
    [Authorize]
    [RequiredScope("Players.Write.All")]
    public async Task<IActionResult> Create(Model.Player player) {
        player.AccountId = AccountId;
        await sport.Add(player);
        return CreatedAtAction(nameof(Get), new {id = player.Id}, player);
    }

    [HttpPut("{id}")]
    [Authorize]
    [RequiredScope("Players.Update.Self")]
    public async Task<IActionResult> Update(long id, Model.Player player) {
        if(id != player.Id || player.AccountId != AccountId) {
            return BadRequest();
        }else if(await sport.Get(id) is not null) {
            await sport.Update(player);
            return NoContent();
        } else return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id) {
        if(await sport.Get(id) is not null) {
            await sport.Delete(id);
            return NoContent();
        } else return NotFound();
    }
}
