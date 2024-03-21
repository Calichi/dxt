using Microsoft.EntityFrameworkCore;

namespace dxt.Data;

public class Sport(DbContextOptions<Sport> options) : DbContext(options)
{
    public DbSet<Models.Player> Players { get; set; }
}
