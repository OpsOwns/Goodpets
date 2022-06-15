namespace Goodpets.API.Validators;

public class UserRegisterValidator : AbstractValidator<UserRegisterRequest>
{
    private readonly string[] _roles = { "admin", "user" };

    public UserRegisterValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Role).Must(x => _roles.Any(z => z == x))
            .WithMessage(_ => "Role of user must be admin or user");
        RuleFor(x => x.UserName).NotEmpty();
    }
}