namespace Goodpets.API.Configuration.Filters;

public class FluentValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
            return;

        var groupedErrors = context.ModelState.Keys.Where(k => context.ModelState[k]!.Errors.Count > 0)
            .Select(key => new { key, messages = context.ModelState[key]!.Errors.Select(x => x.ErrorMessage) })
            .Select(t => new ErrorDetail(t.key, t.messages));

        context.Result = new BadRequestObjectResult(new ErrorResponse(groupedErrors));
    }
}