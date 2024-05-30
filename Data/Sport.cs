using Microsoft.EntityFrameworkCore;

namespace dxt.Database;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<Model.Player> Players { get; set; }
    public DbSet<Model.Team> Teams { get; set; }
    public DbSet<Model.Person> Persons { get; set; }

}
