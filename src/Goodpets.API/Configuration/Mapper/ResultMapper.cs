using Result = FluentResults.Result;

namespace Goodpets.API.Configuration.Mapper;

internal static class ResultMapper
{
    internal static ErrorResponse MapError(this Result result)
    {
        if (result.IsSuccess)
            return new ErrorResponse(Array.Empty<ErrorDetail>());

        return new ErrorResponse(TransformErrors(result.Errors));
    }

    private static IEnumerable<ErrorDetail> TransformErrors(IEnumerable<IError> errors)
    {
        return errors.Select(TransformError);
    }

    private static ErrorDetail TransformError(IError error)
    {
        var errorCode = TransformErrorCode(error);

        return new ErrorDetail(errorCode, error.Message);
    }

    private static string TransformErrorCode(IError error)
    {
        if (error.Metadata.TryGetValue("ErrorParameter", out var errorCode))
            return errorCode as string ?? throw new InvalidOperationException();

        return "Unknown";
    }
}