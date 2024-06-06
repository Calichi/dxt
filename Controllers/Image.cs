using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace dxt.Controller;

[ApiController]
[Route("[controller]")]
public class Image(BlobServiceClient _blob) : ControllerBase
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
            return NotFound($"¡IMAGEN NO ENCONTRADA: {fileName}!");

        try{
        var blob = container.GetBlobClient(fileName);
        var stream = await blob.OpenReadAsync();
        return File(stream, "image/webp");
        } catch (Exception ex) {
            return BadRequest($"ERROR_DE_SERVIDOR_BLOB: {ex}");
        }
    }
}
