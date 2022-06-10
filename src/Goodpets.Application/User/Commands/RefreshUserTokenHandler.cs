// using System.Resources;
//
// namespace Goodpets.Application.User.Commands;
//
// public class RefreshTokenHandler
// {
//     public record RefreshUserToken(string AccessToken, string RefreshToken) : ICommand;
//
//
//     public class RefreshUserTokenHandler : ICommandHandler<RefreshUserToken>
//     {
//         private readonly IUserAccountRepository _accountRepository;
//         private IJwtTokenService _jwtTokenService;
//
//         public RefreshUserTokenHandler(IUserAccountRepository accountRepository, IJwtTokenService jwtTokenService)
//         {
//             _accountRepository = accountRepository;
//             _jwtTokenService = jwtTokenService;
//         }
//
//         public async Task HandleAsync(RefreshUserToken command, CancellationToken cancellationToken = default)
//         {
//             var user = await _accountRepository.GetUserByToken(command.RefreshToken, cancellationToken);
//
//             if (user is null)
//             {
//                 throw new MissingManifestResourceException();
//             }
//
//             var token = _jwtTokenService.Create(user);
//             
//             user.CreateRefreshToken(token.RefreshToken, token.ExpireRefreshToken);
//
//             await _accountRepository.UpdateUser(user, cancellationToken);
//         }
//     }
// }