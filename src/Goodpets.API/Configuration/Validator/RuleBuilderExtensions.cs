namespace Goodpets.API.Configuration.Validator;

public static class RuleBuilderExtensions
{
    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 14)
    {
        var options = ruleBuilder
            .NotEmpty().WithMessage("Password can't be empty")
            .MinimumLength(minimumLength).WithMessage($"Password minimum length required {minimumLength} digits")
            .Matches("[A-Z]").WithMessage("required uppercase letters")
            .Matches("[a-z]").WithMessage("required lowercase letters")
            .Matches("[0-9]").WithMessage("required digits")
            .Matches("[^a-zA-Z0-9]").WithMessage("required special characters");

        return options;
    }

    public static IRuleBuilder<T, string> Role<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        string[] roles = { "admin", "user" };

        var options = ruleBuilder
            .NotEmpty().WithMessage("Password can't be empty")
            .Must(z => roles.Any(x => x == z))
            .WithMessage($"The role must be one of collection elements [{string.Join(',', roles)}]");

        return options;
    }
}