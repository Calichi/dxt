using Microsoft.EntityFrameworkCore;

namespace dxt.Database;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<Model.Player> Players { get; set; }
    public DbSet<Model.Team> Teams { get; set; }
    public DbSet<Model.Person> Persons { get; set; }

    public DbSet<Model.TeamAffiliationRequest> TeamAffiliationRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
              builder.Entity<Model.TeamAffiliationRequest>()
                     .HasOne(e => e.Transmitter)
                     .WithMany(e => e.SentTeamAffiliationRequests)
                     .HasForeignKey("TransmitterId")
                     .IsRequired()
                     .OnDelete(DeleteBehavior.NoAction);

              builder.Entity<Model.TeamAffiliationRequest>()
                     .HasOne(e => e.Receiver)
                     .WithMany(e => e.ReceivedTeamAffiliationRequests)
                     .HasForeignKey("ReceiverId")
                     .IsRequired()
                     .OnDelete(DeleteBehavior.NoAction);

              builder.Entity<Model.Team>()
                     .HasOne(e => e.Owner)
                     .WithMany()
                     .HasForeignKey("OwnerId")
                     .IsRequired()
                     .OnDelete(DeleteBehavior.NoAction);
    }
}
