using Microsoft.EntityFrameworkCore;

namespace dxt.Service;

public class Player(Database.Context sport)
{
    public async Task<List<Model.Player>> GetAll() => await sport.Players.ToListAsync();

    public async Task Add(Model.Player player) {
        await sport.Players.AddAsync(player);
        await sport.SaveChangesAsync();
    }

    public async Task<Model.Player?> Get(string playerId)
    {
        var player = await sport.Players.FindAsync(playerId);

        if (player is not null)
        {
            player.Teams = await sport.Entry(player)
            .Collection(p => p.Teams)
            .Query()
            .Take(1)
            .ToListAsync();
        }
        return player;
    }

    public async Task<bool> Contains(string id) =>
        await sport.Players.FindAsync(id) is not null;

    public async Task Delete(string playerId) {
        if(await Get(playerId) is Model.Player player) {
            sport.Players.Remove(player);
            await sport.SaveChangesAsync();
        }
    }

    public async Task Update(Model.Player player) {
        if(await Get(player.Id) is not null) {
            sport.Players.Update(player);
            await sport.SaveChangesAsync();
        }
    }
}
