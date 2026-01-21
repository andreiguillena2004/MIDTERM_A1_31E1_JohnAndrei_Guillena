using System.Net.Http.Headers;
using System.Text;
using Auth_Level1_Basic.Data;
using Auth_Level1_Basic.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth_Level1_Basic.Middleware;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)//, AppDbContext db)
    {
        // If no Authorization header found, just return 401
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = 401;
            return;
        }

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            var username = credentials[0];
            var password = credentials[1];

            // Verify User
            User user = null; //= await db.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if(username == "admin" && password == "123")
            {
                user = new User { Username = "admin", Password = "123" };
            }

            if (user == null)
            {
                context.Response.StatusCode = 401;
                return;
            }

            // Success! Attach user to context items
            context.Items["User"] = user;
        }
        catch
        {
            context.Response.StatusCode = 401;
            return;
        }

        await _next(context);
    }
}
