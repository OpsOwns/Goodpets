namespace Goodpets.Shared.Api.Dto;

public class ResultDto
{
    public bool IsSuccess { get; }

    public IEnumerable<ErrorDto> Errors { get; set; }

    public ResultDto(bool isSuccess, IEnumerable<ErrorDto> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }
}