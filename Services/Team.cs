﻿using Microsoft.EntityFrameworkCore;

namespace dxt.Service;

public class Team(Database.Context db)
{
    public async void Add(Model.Team team)
    {
        var player = (await db.Players.FindAsync(team.Players?.First().Id))!;
        team.Players = [];
        player.Teams = [team];
        db.SaveChanges();
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
