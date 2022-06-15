namespace Goodpets.Application.Dto.Results;

public class ResultDto
{
    public ResultDto(bool isSuccess, IEnumerable<ErrorDto> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    public bool IsSuccess { get; }

    public IEnumerable<ErrorDto> Errors { get; set; }
}