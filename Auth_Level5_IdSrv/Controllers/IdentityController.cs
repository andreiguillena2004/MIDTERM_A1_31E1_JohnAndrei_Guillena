using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth_Level5_IdSrv.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "ApiScope")] // Enforce the Policy defined in Program.cs
public class IdentityController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "You have accessed the IdentityServer Protected API!",
            user = User.Identity.Name,
            claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
