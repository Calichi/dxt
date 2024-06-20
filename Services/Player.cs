using Microsoft.EntityFrameworkCore;

namespace dxt.Service;

public class Player(Database.Context sport)
{
    public async Task<List<Model.Player>> GetAll() => await sport.Players.ToListAsync();

    public async Task Add(Model.Player player) {
        await sport.Players.AddAsync(player);
        await sport.SaveChangesAsync();
    }

    public Model.Player? GetByUniqueId(string uniqueId) =>
        sport.Players.FirstOrDefault( p => p.UniqueId == uniqueId );

    public async Task<Model.Player?> Get(string uniqueId)
    {
        if (GetByUniqueId(uniqueId) is Model.Player player)
        {
            player.Teams = await sport.Entry(player)
            .Collection(p => p.Teams)
            .Query()
            .Take(1)
            .ToListAsync();
            return player;
        }
        return null;
    }

    public bool Contains(string uniqueId) => GetByUniqueId(uniqueId) is not null;

    public void Delete(Model.Player player)
    {
        sport.Players.Remove(player);
        sport.SaveChanges();
    }


    public void Update(Model.Player player)
    {
        sport.Players.Update(player);
        sport.SaveChanges();
    }
}
