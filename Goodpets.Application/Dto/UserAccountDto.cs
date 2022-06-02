namespace Goodpets.Application.Dto;

public record UserAccountDto
{
    private string Email { get; }
    private string UserName { get; }
    private string Role { get; }
    public static UserAccountDto Empty => new();

    private UserAccountDto()
    {
        Email = null!;
        UserName = null!;
        Role = null!;
    }

    public UserAccountDto(string email, string userName, string role)
    {
        Email = email;
        UserName = userName;
        Role = role;
    }
}