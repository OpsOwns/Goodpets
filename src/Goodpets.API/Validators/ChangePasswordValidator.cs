namespace Goodpets.API.Validators;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.NewPassword).Password(8);
        RuleFor(x => x.OldPassword).Password(8);
    }
}