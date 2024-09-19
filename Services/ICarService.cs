public interface ICarService
{
    Task<IEnumerable<Car>> GetCarsAsync(int dealerId);
    Task<Car?> GetCarByIdAsync(int id, int dealerId);
    Task<Car> AddCarAsync(AddCar car, int dealerId);
    Task<bool> UpdateCarStockAsync(int id, int newStock, int dealerId);
    Task<bool> DeleteCarAsync(int id, int dealerId);
    Task<IEnumerable<Car>> SearchCarsAsync(int dealerId, string make, string model);
}
