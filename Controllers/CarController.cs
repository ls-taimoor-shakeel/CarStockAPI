using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class CarsController : BaseController
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    // GET: api/cars - List all cars for the authenticated dealer
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Car>>> GetCars()
    {
        var dealerId = GetDealerId();
        var cars = await _carService.GetCarsAsync(dealerId);

        if (cars == null || !cars.Any())
        {
            return NotFound("No cars found for this dealer.");
        }

        return Ok(cars);
    }

    // GET: api/cars/{id} - Get a specific car by ID for the authenticated dealer
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCarById(int id)
    {
        var dealerId = GetDealerId();
        var car = await _carService.GetCarByIdAsync(id, dealerId);

        if (car == null)
        {
            return NotFound("Car not found");
        }

        return Ok(car);
    }

    // POST: api/cars - Add a new car for the authenticated dealer
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Car>> AddCar([FromBody] AddCar car)
    {
        var dealerId = GetDealerId();
        var newCar = await _carService.AddCarAsync(car, dealerId);

        return CreatedAtAction(nameof(GetCarById), new { id = newCar.ID }, newCar);
    }

    // PUT: api/cars/{id}/stock - Update the stock level of a car for the authenticated dealer
    [Authorize]
    [HttpPut("{id}/stock")]
    public async Task<IActionResult> UpdateCarStock(int id, [FromBody] int newStock)
    {
        var dealerId = GetDealerId();

        var success = await _carService.UpdateCarStockAsync(id, newStock, dealerId);

        if (!success)
        {
            return NotFound("Car not found or unauthorized access.");
        }

        return Ok("Stock Updated Successfully");
    }

    // DELETE: api/cars/{id} - Delete a car for the authenticated dealer
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var dealerId = GetDealerId();
        var success = await _carService.DeleteCarAsync(id, dealerId);

        if (!success)
        {
            return NotFound("Car not found or unauthorized access.");
        }

        return Ok("Car Deleted Successfully.");
    }

    // GET: api/cars/search?make=Toyota&model=Corolla - Search cars by make and/or model for the authenticated dealer
    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Car>>> SearchCars([FromQuery] string? make, [FromQuery] string? model)
    {
        var dealerId = GetDealerId();
        var cars = await _carService.SearchCarsAsync(dealerId, make, model);

        if (!cars.Any())
        {
            return NotFound("No cars found matching the search criteria for this dealer.");
        }

        return Ok(cars);
    }
}
