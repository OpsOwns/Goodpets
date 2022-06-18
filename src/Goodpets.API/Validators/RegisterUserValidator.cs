namespace Goodpets.API.Validators;

public class RegisterUserValidator : AbstractValidator<UserRegisterRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).Password(8);
        RuleFor(x => x.Role).Role();
        RuleFor(x => x.UserName).MinimumLength(3).NotEmpty();
    }
}