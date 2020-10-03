using AltiumTest.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AltiumTest.Data.EF.SQLServer
{
  public class ApplicationContext : DbContext
  {
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Message>(etb =>
      {
        etb.HasKey(e => e.Id);

        etb.Property(e => e.Id).ValueGeneratedOnAdd();
        etb.Property(e => e.Created).IsRequired().HasColumnType("datetime2(7)");
        etb.Property(e => e.UserName).IsRequired();
        etb.Property(e => e.Text).IsRequired();

        etb.HasIndex(x => x.Created);
        etb.ToTable("Messages");
      });
    }

    public DbSet<Message> Messages { get; set; }
  }
}
