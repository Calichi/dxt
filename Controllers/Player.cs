using Microsoft.AspNetCore.Mvc;

namespace dxt.Controllers;

[ApiController]
[Route("[controller]")]
public class Player(Services.Player sport) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Models.Player>>> Get() =>
        await sport.GetAll();

    [HttpGet("{id}")]
    public async Task<ActionResult<Models.Player>> Get(long id) {
        if(await sport.Get(id) is Models.Player player)
            return player;
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Models.Player player) {
        await sport.Add(player);
        return CreatedAtAction(nameof(Get), new {id = player.Id}, player);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, Models.Player player) {
        if(id != player.Id) return BadRequest();
        else if(await sport.Get(id) is not null) {
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
