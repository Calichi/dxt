using Microsoft.AspNetCore.Mvc;

namespace dxt.Controllers;

[ApiController]
[Route("[controller]")]
public class Player : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Models.Player>> Get() =>
        Services.Player.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Models.Player> Get(long id) {
        if(Services.Player.Get(id) is Models.Player player)
            return player;
        return NotFound();
    }

    [HttpPost]
    public IActionResult Create(Models.Player player) {
        Services.Player.Add(player);
        return CreatedAtAction(nameof(Get), new {id = player.Id}, player);
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, Models.Player player) {
        if(id != player.Id) return BadRequest();
        else if(Services.Player.Get(id) is not null) {
            Services.Player.Update(player);
            return NoContent();
        } else return NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id) {
        if(Services.Player.Get(id) is not null) {
            Services.Player.Delete(id);
            return NoContent();
        } else return NotFound();
    }
}
