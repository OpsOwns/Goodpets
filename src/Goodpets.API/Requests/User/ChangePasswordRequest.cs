namespace Goodpets.API.Requests.User;

public record ChangePasswordRequest(string OldPassword, string NewPassword);