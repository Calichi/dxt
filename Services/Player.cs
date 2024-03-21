using Microsoft.EntityFrameworkCore;

namespace dxt.Services;

public class Player(Data.Sport sport)
{
    public async Task<List<Models.Player>> GetAll() => await sport.Players.ToListAsync();

    public async Task Add(Models.Player player) {
        await sport.Players.AddAsync(player);
        await sport.SaveChangesAsync();
    }

    public async Task<Models.Player?> Get(long playerId) =>
        await sport.Players.FirstOrDefaultAsync(item => item.Id == playerId);

    public async Task Delete(long playerId) {
        if(await Get(playerId) is Models.Player player) {
            sport.Players.Remove(player);
            await sport.SaveChangesAsync();
        }
    }

    public async Task Update(Models.Player player) {
        if(await Get(player.Id) is not null) {
            sport.Players.Update(player);
            await sport.SaveChangesAsync();
        }
    }
}
