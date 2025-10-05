using Microsoft.EntityFrameworkCore;

namespace dxt.Database;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<Model.Player> Players { get; set; }
    public DbSet<Model.Team> Teams { get; set; }
    public DbSet<Model.Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Model.TeamAffiliationRequest>()
               .HasOne(e => e.Transmitter)
               .WithMany(e => e.SentTeamAffiliationRequests)
               .HasForeignKey("TransmitterId")
               .IsRequired();

        builder.Entity<Model.TeamAffiliationRequest>()
               .HasOne(e => e.Receiver)
               .WithMany(e => e.ReceivedTeamAffiliationRequests)
               .HasForeignKey("ReceiverId")
               .IsRequired();
    }
}
