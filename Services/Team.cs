using Microsoft.EntityFrameworkCore;

namespace dxt.Service;

public class Team(Database.Context db)
{
    public async Task AddAsync(Model.Team team)
    {
        await db.Teams.AddAsync( team );
        await db.SaveChangesAsync();
    }

    public async Task<bool> ContainsAsync(Model.Team team) =>
        await db.Teams.AnyAsync( entry => entry.SearchId == team.SearchId );

    public async Task<Model.Team?> GetAsync(long id) =>
        await db.Teams.FirstOrDefaultAsync(item => item.Id == id);

    public async Task<List<Model.Team>> GetAllAsync()
    {
        return await db.Teams.ToListAsync();
    }
}
