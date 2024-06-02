using Microsoft.EntityFrameworkCore;

namespace dxt.Service;

public class Team(Database.Context db)
{
    public async Task AddAsync(Model.Team team)
    {
        await db.AddAsync( team );
        await db.SaveChangesAsync();
    }

    public async Task<bool> ContainsAsync(Model.Team team) =>
        await db.Teams.AnyAsync( entry => entry.SearchId == team.SearchId );

    public async Task<Model.Team?> GetAsync(long id) =>
        await db.Teams.FindAsync( id );

    public async Task<ICollection<Model.Team>> GetAsync()
    {
        return await db.Teams.ToListAsync();
    }
}
