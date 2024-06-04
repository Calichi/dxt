using Microsoft.EntityFrameworkCore;

namespace dxt.Service;

public class Team(Database.Context db)
{
    public void Add(Model.Team team)
    {
        // var player = db.Players.Find(team.Players?.First().Id)!;
        // team.Players = [];
        // player.Teams = [team];
        db.Update(team);
        db.SaveChanges();
    }

    public bool Contains(Model.Team team) =>
        db.Teams.Any( entry => entry.SearchId == team.SearchId );

    public Model.Team? Get(long id) =>
        db.Teams.Find(id);

    public async Task<List<Model.Team>> GetAllAsync()
    {
        return await db.Teams.ToListAsync();
    }
}
