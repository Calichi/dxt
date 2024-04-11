using Microsoft.EntityFrameworkCore;

namespace dxt.Services;

public class Player(Data.Sport sport)
{
    public async Task<List<Model.Player>> GetAll() => await sport.Players.ToListAsync();

    public async Task Add(Model.Player player) {
        await sport.Players.AddAsync(player);
        await sport.SaveChangesAsync();
    }

    public async Task<Model.Player?> Get(long playerId) =>
        await sport.Players.FirstOrDefaultAsync(item => item.Id == playerId);

    public async Task Delete(long playerId) {
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
