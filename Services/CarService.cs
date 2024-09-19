using CarStockAPI.Data;
using Microsoft.EntityFrameworkCore;

public class CarService : ICarService
{
    private readonly CarContext _context;

    public CarService(CarContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Car>> GetCarsAsync(int dealerId)
    {
        return await _context.Cars
            .Where(c => c.DealerId == dealerId)
            .ToListAsync();
    }

    public async Task<Car?> GetCarByIdAsync(int id, int dealerId)
    {
        return await _context.Cars
            .FirstOrDefaultAsync(c => c.ID == id && c.DealerId == dealerId);
    }

    public async Task<Car> AddCarAsync(AddCar car, int dealerId)
    {
        var newCar = new Car()
        {
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            Stock = car.Stock,
            DealerId = dealerId
        };
        _context.Cars.Add(newCar);
        await _context.SaveChangesAsync();
        return newCar;
    }
    public async Task<bool> UpdateCarStockAsync(int id, int newStock, int dealerId)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.ID == id && c.DealerId == dealerId);

        if (car == null || newStock < 0)
        {
            return false;
        }

        car.Stock = newStock;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCarAsync(int id, int dealerId)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.ID == id && c.DealerId == dealerId);

        if (car == null)
        {
            return false;
        }

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Car>> SearchCarsAsync(int dealerId, string? make, string? model)
    {
        var query = _context.Cars.Where(c => c.DealerId == dealerId).AsQueryable();

        if (!string.IsNullOrEmpty(make))
        {
            query = query.Where(c => c.Make.Contains(make, System.StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(model))
        {
            query = query.Where(c => c.Model.Contains(model, System.StringComparison.OrdinalIgnoreCase));
        }

        return await query.ToListAsync();
    }
}
