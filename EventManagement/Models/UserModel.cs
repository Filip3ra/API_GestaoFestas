namespace Auth.Models;

public class UserModel
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}
