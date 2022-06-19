namespace Goodpets.API.Validators;

public class SignUpValidator : AbstractValidator<SignUpRequest>
{
    public SignUpValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).Password(8);
        RuleFor(x => x.Role).Role();
        RuleFor(x => x.UserName).MinimumLength(3).NotEmpty();
    }
}