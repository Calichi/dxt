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
    [Authorize("Players.Read.All")]
    public async Task<ActionResult<List<Models.Player>>> Get() =>
        await sport.GetAll();

    [HttpGet("{id}")]
    [Authorize("Players.Read.All")]
    public async Task<ActionResult<Models.Player>> Get(long id) {
        if(await sport.Get(id) is Models.Player player)
            return player;
        return NotFound();
    }

    [HttpPost]
    [Authorize("Players.Write.All")]
    public async Task<IActionResult> Create(Models.Player player) {
        player.AccountId = AccountId;
        await sport.Add(player);
        return CreatedAtAction(nameof(Get), new {id = player.Id}, player);
    }

    [HttpPut("{id}")]
    [Authorize("Players.Update.Self")]
    public async Task<IActionResult> Update(long id, Models.Player player) {
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
