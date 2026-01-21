# ASP.NET Core Authentication Samples

This folder contains 5 distinct projects demonstrating different authentication strategies. Each project is standalone and configured to use SQLite.

## How to Run a Project

1.  Open your terminal.
2.  Navigate to the project folder (e.g., `cd Auth_Level1_Basic`).
3.  Run the application:
    ```powershell
    dotnet run
    ```
4.  The application will start (usually on `https://localhost:7000` or similar). Check the console output for the exact URL.

## Database Setup (SQLite)

**Great news!** You do **NOT** need to run manual migrations to start these projects.

All projects are configured with the following code in `Program.cs`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // This creates the 'auth.db' file automatically if it doesn't exist
    db.Database.EnsureCreated();
}
```

Just running `dotnet run` will create the `auth.db` file in the project folder and seed it with a default Admin user:
*   **Username:** `admin`
*   **Password:** `123`

### Making Changes (Migrations)
If you decide to modify the `User` model later (e.g., adding an `Email` property), you will need to perform standard EF Core migrations:

1.  **Install the Tool (Global):**
    ```powershell
    dotnet tool install --global dotnet-ef
    ```

2.  **Add a Migration:**
    ```powershell
    dotnet ef migrations add AddEmailField
    ```

3.  **Update Database:**
    ```powershell
    dotnet ef database update
    ```

## Project Overview

| Project | Level | Description |
| :--- | :--- | :--- |
| **Auth_Level1_Basic** | Beginner | Uses a custom Middleware to check `Authorization: Basic` headers. |
| **Auth_Level2_Cookie** | Intermediate | Uses ASP.NET Core's built-in Cookie Auth schema. Great for standard web apps. |
| **Auth_Level3_JWT** | Advanced | Generates and Validates JSON Web Tokens. The standard for APIs. |
| **Auth_Level4_RBAC** | Expert | Adds Role-Based Access Control (`[Authorize(Roles="Admin")]`) on top of JWT. |
| **Auth_Level5_IdSrv** | Architect | Configured to trust an external IdentityServer (OAuth 2.0). |

## Testing
Please refer to the `ASP_NET_Core_Auth_Guide.md` file in the parent directory for specific **Postman** testing instructions for each level.
