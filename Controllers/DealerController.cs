using FluentValidation;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DealerController : ControllerBase
{
    private readonly IDealerService _dealerService;
    private readonly IValidator<RegisterDealer> _registerValidator;

    public DealerController(IDealerService dealerService, IValidator<RegisterDealer> registerValidator)
    {
        _dealerService = dealerService;
        _registerValidator = registerValidator;
    }

    // POST: api/dealer/register - Register a new dealer
    [HttpPost("register")]
    public async Task<ActionResult> RegisterDealer([FromBody] RegisterDealer dealer)
    {
        try
        {
            var registeredDealer = await _dealerService.RegisterDealer(dealer);
            return Ok(new { registeredDealer.DealerId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/dealer/login - Login a dealer
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDealer loginDetails)
    {
        try
        {
            var token = await _dealerService.Login(loginDetails);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}
