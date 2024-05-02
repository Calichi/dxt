using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace dxt.Controllers;

[ApiController]
[Route("player/[controller]")]
public class ImageController(BlobServiceClient _blob) : ControllerBase
{
    [HttpPost("{id}")]
    [Authorize]
    [RequiredScope("Players.Write.All")]
    public async Task<ActionResult> Create(string id, IFormFile image) {
        var container = _blob.GetBlobContainerClient(id);

        if(! await container.ExistsAsync())
            await container.CreateIfNotExistsAsync();

        var blob = container.GetBlobClient(image.FileName);
        await blob.UploadAsync(image.OpenReadStream(), true);
        return Content(blob.Uri.ToString());
    }

    [HttpGet("{id}")]
    [Authorize]
    [RequiredScope("Players.Read.All")]
    public async Task<IActionResult> Get(string id, string fileName) {
        var container = _blob.GetBlobContainerClient(id);
        if(! await container.ExistsAsync())
            return NotFound("¡IMAGEN NO ENCONTRADA!");

        var blob = container.GetBlobClient(fileName);
        using var stream = await blob.OpenReadAsync();
        return File(stream, "image/webp");
    }
}
