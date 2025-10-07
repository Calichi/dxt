using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace dxt.Service;

public class Team(Database.Context db)
{
    public void Add(Model.Team team)
    {
        db.Update( team );
        db.SaveChanges();
    }

    public bool Contains(Model.Team team) =>
        db.Teams.Any( entry => entry.SearchId == team.SearchId );

    public Model.Team? Get(long id, bool withPlayers = false)
    {
        if(!withPlayers) return db.Teams.Find(id);

        return db.Teams
                 .Include( entry => entry.Players)
                 .FirstOrDefault( entry => entry.Id == id );
    }

    public async Task<List<Model.Team>> GetAllAsync()
    {
        return await db.Teams.ToListAsync();
    }

    public List<Model.Team> Get(string searchId)
    {
        return [..
            from team in db.Teams.Include(t => t.Owner)
            where team.SearchId.ToLower().Contains(searchId.ToLower())
            select team
        ];
    }
}
