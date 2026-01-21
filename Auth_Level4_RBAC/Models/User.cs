namespace Auth_Level4_RBAC.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; 
    public string Role { get; set; } = "User";
}
