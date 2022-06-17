namespace Goodpets.API.Configuration.Filters;

public class FluentValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
            return;

        var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
            .SelectMany(v => v.Errors)
            .Select(v => new ErrorDetail("Request", v.ErrorMessage))
            .ToList();

        context.Result = new BadRequestObjectResult(new ErrorResponse(errors));
    }
}