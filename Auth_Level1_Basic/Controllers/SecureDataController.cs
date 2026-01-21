using Microsoft.AspNetCore.Mvc;

namespace Auth_Level1_Basic.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SecureDataController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // We know user exists because Middleware verified it
        return Ok("You have accessed the Secure Data!");
    }
}
