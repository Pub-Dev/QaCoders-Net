using Microsoft.EntityFrameworkCore;

namespace QaCoders_Net.Configuration;

public class QaCodersDbContext : DbContext
{
    public QaCodersDbContext(DbContextOptions<QaCodersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QaCodersDbContext).Assembly);
    }
}
