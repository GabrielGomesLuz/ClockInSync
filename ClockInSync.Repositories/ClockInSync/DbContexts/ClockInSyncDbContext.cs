using ClockInSync.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClockInSync.Repositories.DbContexts
{
    public class ClockInSyncDbContext(DbContextOptions<ClockInSyncDbContext> options) : DbContext(options)
    {

        public DbSet<User> Users { get; set; } = default!;

        public DbSet<PunchClock> PunchClocks { get; set; } = default!;

        public DbSet<Settings> Settings { get; set; } = default!;

    }
}
