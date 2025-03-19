using Microsoft.EntityFrameworkCore;
using udvSummerSchoolTestTask.Entities;

namespace udvSummerSchoolTestTask.DataBases;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<LetterEntity> Letters { get; set; }
}