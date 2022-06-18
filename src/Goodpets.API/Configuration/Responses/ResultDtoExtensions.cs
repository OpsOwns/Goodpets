namespace Goodpets.API.Configuration.Responses;

public static class ResultDtoExtensions
{
    public static ErrorResponse MapToError(this Result result)
    {
        if (result.IsSuccess)
            return new ErrorResponse(Enumerable.Empty<ErrorDetail>());

        return new ErrorResponse(TransformErrors(result.Errors));
    }

    private static IEnumerable<ErrorDetail> TransformErrors(IEnumerable<IError> errors)
    {
        return errors.Select(TransformError);
    }

    private static ErrorDetail TransformError(IError error)
    {
        var errorCode = TransformErrorCode(error);

        return new ErrorDetail(error.Message, errorCode);
    }

    private static string TransformErrorCode(IError error)
    {
        if (error.Metadata.TryGetValue("ErrorCode", out var errorCode))
            return errorCode as string ?? throw new InvalidOperationException();

        return "Undefined";
    }
}