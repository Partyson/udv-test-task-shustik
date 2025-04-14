using Microsoft.EntityFrameworkCore;
using udvSummerSchoolTestTask.Entities;

namespace udvSummerSchoolTestTask.DataBases;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<StatisticEntity> Letters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StatisticEntity>()
            .HasKey(x => x.Id);
    }
}