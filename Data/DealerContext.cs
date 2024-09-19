using Microsoft.EntityFrameworkCore;

public class DealerContext : DbContext
{
    public DbSet<Dealer> Dealers { get; set; }

    public DealerContext(DbContextOptions<DealerContext> options)
        : base(options) { }
}
