using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class CarsControllerTests
{
    private readonly Mock<ICarService> _mockCarService;
    private readonly CarsController _controller;

    public CarsControllerTests()
    {

        _mockCarService = new Mock<ICarService>();

        _controller = new CarsController(_mockCarService.Object);

        // Mock the HttpContext and set the User with a DealerId claim
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetCars_ReturnsOkResult_WhenCarsExist()
    {
        // 
        var dealerId = 1;
        var cars = new List<Car> { new Car { ID = 1, Make = "Toyota", Model = "Corolla", DealerId = dealerId } };
        _mockCarService.Setup(service => service.GetCarsAsync(dealerId)).ReturnsAsync(cars);

        var result = await _controller.GetCars();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Car>>(okResult.Value);
        Assert.Equal(1, returnValue.Count);
    }

    [Fact]
    public async Task GetCars_ReturnsNotFound_WhenNoCarsExist()
    {
        // 
        var dealerId = 1;
        _mockCarService.Setup(service => service.GetCarsAsync(dealerId)).ReturnsAsync(new List<Car>());

        var result = await _controller.GetCars();

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddCar_ReturnsCreatedResult_WhenCarIsAdded()
    {
        // 
        var dealerId = 1;
        var car = new AddCar { Make = "Toyota", Model = "Corolla", Year = 2020, Stock = 5 };
        var newCar = new Car { ID = 1, Make = "Toyota", Model = "Corolla", Year = 2020, Stock = 5, DealerId = dealerId };

        _mockCarService.Setup(service => service.AddCarAsync(car, dealerId)).ReturnsAsync(newCar);

        var result = await _controller.AddCar(car);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetCarById", createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task UpdateCarStock_ReturnsOkResult_WhenStockUpdated()
    {
        // 
        var dealerId = 1;
        var carId = 1;
        var newStock = 10;
        _mockCarService.Setup(service => service.UpdateCarStockAsync(carId, newStock, dealerId)).ReturnsAsync(true);

        var result = await _controller.UpdateCarStock(carId, newStock);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCarStock_ReturnsNotFound_WhenCarNotFound()
    {
        // 
        var dealerId = 1;
        var carId = 1;
        var newStock = 10;
        _mockCarService.Setup(service => service.UpdateCarStockAsync(carId, newStock, dealerId)).ReturnsAsync(false);

        var result = await _controller.UpdateCarStock(carId, newStock);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteCar_ReturnsOkResult_WhenCarDeleted()
    {
        // 
        var dealerId = 1;
        var carId = 1;
        _mockCarService.Setup(service => service.DeleteCarAsync(carId, dealerId)).ReturnsAsync(true);

        var result = await _controller.DeleteCar(carId);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteCar_ReturnsNotFound_WhenCarNotFound()
    {
        // 
        var dealerId = 1;
        var carId = 1;
        _mockCarService.Setup(service => service.DeleteCarAsync(carId, dealerId)).ReturnsAsync(false);

        var result = await _controller.DeleteCar(carId);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Search_ReturnsOkResult_WhenCarsAreFound()
    {
        // 
        var dealerId = 1;
        var make = "Toyota";
        var model = "Corolla";

        // Mock list of cars returned by the search
        var cars = new List<Car>
    {
        new Car { ID = 1, Make = make, Model = model, Year = 2020, DealerId = dealerId }
    };

        // Set up the mock car service to return the mock list of cars
        _mockCarService.Setup(service => service.SearchCarsAsync(dealerId, make, model))
                       .ReturnsAsync(cars);

        var result = await _controller.SearchCars(make, model);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);  // Expecting OK result
        var returnValue = Assert.IsType<List<Car>>(okResult.Value);  // Expecting list of cars
        Assert.Single(returnValue);  // There should be exactly one car in the result
        Assert.Equal(make, returnValue[0].Make);  // Validate that the returned car matches the search parameters
    }

    [Fact]
    public async Task Search_ReturnsNotFound_WhenNoCarsAreFound()
    {

        var dealerId = 1;
        var make = "Toyota";
        var model = "Corolla";

        // Set up the mock car service to return an empty list (no cars found)
        _mockCarService.Setup(service => service.SearchCarsAsync(dealerId, make, model))
                       .ReturnsAsync(new List<Car>());
                       
        var result = await _controller.SearchCars(make, model);

        Assert.IsType<NotFoundObjectResult>(result.Result);  // Expecting 404 Not Found
    }

}
