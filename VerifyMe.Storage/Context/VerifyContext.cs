using Microsoft.EntityFrameworkCore;
using VerifyMe.Models.DLA;

namespace VerifyMe.Storage.Context;

public sealed class VerifyContext : DbContext
{
    public VerifyContext() => Database.MigrateAsync();
    public DbSet<App> Apps { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Sms> Sms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = app.db");
    }
}