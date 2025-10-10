using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace dxt.Controllers;

using TAR = Model.TeamAffiliationRequest;

[ApiController]
[Route("[controller]")]
public class TeamAffiliationRequest(
  Services.TeamAffiliationRequest tarSvc
) : ControllerBase
{
  [Authorize, HttpPost, RequiredScope("Players.Write.All")]
  public async Task<ActionResult<TAR>> Create(TAR tar)
  {
    if (tar is { Id: not 0 })
      return Conflict("The entity's Id must not be pre-assigned.");

    await tarSvc.SaveAsync(tar);
    return Created(string.Empty, tar);
  }

  [Authorize, HttpPatch, RequiredScope("Players.Write.All")]
  public async Task<ActionResult<TAR>> UpdateAsync(TAR tar)
  {
    if (tar is { Id: 0 })
      return Conflict("The entity's Id must not be 0");

    await tarSvc.UpdateAsync(tar);
    return Ok(tar);
  }

  [Authorize, HttpPatch("{amphirionId}"), RequiredScope("Players.Write.All")]
  public async Task<ActionResult> AffiliateAsync(long amphirionId, Model.Player player)
  {
    await tarSvc.Affiliate(amphirionId, player);
    return Ok();
  }
}