namespace Goodpets.API.Validators;

public static class FluentValidatorExtensions
{
    public static Action<FluentValidationMvcConfiguration> AddConfiguration()
    {
        return config =>
        {
            config.ValidatorOptions.LanguageManager.Culture = new CultureInfo("en-US");
            config.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        };
    }
}