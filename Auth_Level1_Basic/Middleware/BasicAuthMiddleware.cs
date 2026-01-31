using System.Net;
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

    public async Task Invoke(HttpContext context, AppDbContext db)
    {
        // 1️⃣ Check for Authorization header
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Missing Authorization header");
            return;
        }

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
            if (authHeader.Scheme != "Basic")
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Only Basic auth is allowed");
                return;
            }

            // 2️⃣ Decode Base64
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? "");
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

            if (credentials.Length != 2)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid credentials format");
                return;
            }

            var username = credentials[0];
            var password = credentials[1];

            // 3️⃣ Lookup in SQLite Users table
            var user = await db.Users.FirstOrDefaultAsync(u =>
                u.Username == username && u.Password == password);

            if (user == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid username or password");
                return;
            }

            // 4️⃣ Success: attach user to context
            context.Items["User"] = user;
        }
        catch
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Invalid Authorization header");
            return;
        }

        // Continue to next middleware
        await _next(context);
    }
}
