using dxt.Database;
using Microsoft.EntityFrameworkCore;

namespace dxt.Services;

using TAR = Model.TeamAffiliationRequest;

public class TeamAffiliationRequest(Context c)
{
  public async Task<TAR> SaveAsync(TAR tar)
  {
    await c.AddAsync(tar);
    await c.SaveChangesAsync();    
    return tar;
  }

  public async Task<TAR> UpdateAsync(TAR tar)
  {
    c.Update(tar);
    await c.SaveChangesAsync();
    return tar;
  }
  
  public async Task Affiliate(long amphirionId, Model.Player player)
  {
    var team = c.Teams.Include(e => e.Owner)
                      .SingleOrDefault(t => t.Id == amphirionId);
                      
    if (team is not null)
    {
      await c.Entry(team)
            .Collection(t => t.Players)
            .LoadAsync();

      team.Players.Add(player);
      await c.SaveChangesAsync();
    }

  }
}
