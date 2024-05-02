using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Azure.Storage.Blobs;

namespace dxt.Controllers;

[ApiController]
[Route("[controller]")]
public class Player(Services.Player dtoPlayer, BlobServiceClient _blob) : ControllerBase
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
    public async Task<IActionResult> Create(Model.Player model) {
        if(await dtoPlayer.Contains(model.Id))
            return Conflict("¡Esta cuenta ya esta registrada!");

        await dtoPlayer.Add(model);
        return CreatedAtAction(nameof(Get), new {id = model.Id}, model);
    }

    [HttpPut("{id}")]
    [Authorize]
    [RequiredScope("Players.Update.Self")]
    public async Task<IActionResult> Update(string id, Model.Player player) {
        if(id != player.Id) {
            return BadRequest();
        }else if(await dtoPlayer.Get(id) is not null) {
            await dtoPlayer.Update(player);
            return NoContent();
        } else return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) {
        if(await dtoPlayer.Get(id) is not null) {
            await dtoPlayer.Delete(id);
            return NoContent();
        } else return NotFound();
    }
}
