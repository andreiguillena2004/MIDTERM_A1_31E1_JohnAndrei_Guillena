namespace Auth_Level1_Basic.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // In production, use Hashing!
    public string Role { get; set; } = "User";
}
