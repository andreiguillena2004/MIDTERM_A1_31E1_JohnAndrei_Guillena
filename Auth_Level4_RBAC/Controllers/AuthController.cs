using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth_Level4_RBAC.Data;
using Auth_Level4_RBAC.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth_Level4_RBAC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private const string SecretKey = "MySuperSecretKeyThatIsLongEnoughToSatisfyHMACSHA256!";

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto request)
    {
        var user = _db.Users.SingleOrDefault(u => u.Username == request.Username && u.Password == request.Password);
        if (user == null) return Unauthorized("Invalid Credentials");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = jwt });
    }

    // Accessible by ANY logged in user (Admin or User)
    [Authorize]
    [HttpGet("public-profile")]
    public IActionResult GetProfile()
    {
        return Ok($"Hello {User.Identity.Name}, you are logged in.");
    }

    // Accessible ONLY by Admin
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete-database")]
    public IActionResult DeleteDatabase()
    {
        return Ok("DATABASE DELETED! (Not really, but you are authorized to do it)");
    }
}
