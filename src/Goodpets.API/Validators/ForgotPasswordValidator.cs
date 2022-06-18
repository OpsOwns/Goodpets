namespace Goodpets.API.Validators;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
    }
}