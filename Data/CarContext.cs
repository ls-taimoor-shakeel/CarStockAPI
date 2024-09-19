using Microsoft.EntityFrameworkCore;

namespace CarStockAPI.Data
{
    public class CarContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public CarContext(DbContextOptions<CarContext> options)
            : base(options) { }
    }
}
