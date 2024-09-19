using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class BaseController : ControllerBase
{
    // Method to get DealerID from JWT token
    protected int GetDealerId()
    {
        var dealerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(dealerId);
    }
}
