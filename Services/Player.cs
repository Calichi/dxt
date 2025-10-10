using Microsoft.EntityFrameworkCore;

namespace dxt.Service;

public class Player(Database.Context sport)
{
  public async Task<List<Model.Player>> GetAll() => await sport.Players.ToListAsync();

  public async Task Add(Model.Player player)
  {
    await sport.Players.AddAsync(player);
    await sport.SaveChangesAsync();
  }

  public Model.Player? GetByUniqueId(string uniqueId) =>
      sport.Players.FirstOrDefault(p => p.UniqueId == uniqueId);

  public async Task<Model.Player?> Get(string uniqueId)
  {
    var player = await sport.Players
        .Include(p => p.ReceivedTeamAffiliationRequests)
        .Include(p => p.SentTeamAffiliationRequests)
        .FirstOrDefaultAsync(p => p.UniqueId == uniqueId);

    if (player is null) return null;

    await sport.Entry(player)
        .Collection(p => p.Teams)
        .Query()
        .Take(1)
        .LoadAsync();
    
    return player;
  }

  public bool Contains(string uniqueId) => GetByUniqueId(uniqueId) is not null;

  public void Update(Model.Player player)
  {
    sport.Players.Update(player);
    sport.SaveChanges();
  }

  public async Task DeleteAsync(Model.Player player)
  {
    var strategy = sport.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
      await using var transaction = await sport.Database.BeginTransactionAsync();
      try
      {
        // First delete player dependencies
        await
            sport.Teams
                .Where(t => EF.Property<long>(t, "OwnerId") == player.Id)
                .ExecuteDeleteAsync();

        await
            sport.Entry(player)
                .Collection(e => e.SentTeamAffiliationRequests)
                .Query()
                .ExecuteDeleteAsync();

        await
            sport.Entry(player)
                .Collection(e => e.ReceivedTeamAffiliationRequests)
                .Query()
                .ExecuteDeleteAsync();

        // Then delete player
        await
            sport.Players
                .Where(p => p.Id == player.Id)
                .ExecuteDeleteAsync();

        await transaction.CommitAsync();
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    });
  }
}
