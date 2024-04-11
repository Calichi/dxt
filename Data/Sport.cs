using Microsoft.EntityFrameworkCore;

namespace dxt.Data;

public class Sport(DbContextOptions<Sport> options) : DbContext(options)
{
    public DbSet<Model.Player> Players { get; set; }
}
