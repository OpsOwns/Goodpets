﻿namespace Goodpets.Application.User.Queries;

public record GetUserAccountById(UserAccountId UserAccountId) : IQuery<UserAccountDto>;

public class GetUserAccountByIdHandler : IQueryHandler<GetUserAccountById, UserAccountDto>
{
    private readonly IUserAccountRepository _accountRepository;

    public GetUserAccountByIdHandler(IUserAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }


    public async Task<UserAccountDto> HandleAsync(GetUserAccountById query, CancellationToken cancellationToken)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var account = await _accountRepository.GetUserAccount(query.UserAccountId, cancellationToken);

        if (!account.Exists)
            return UserAccountDto.Empty;

        return new UserAccountDto(account.Email.Value, account.Credentials.Username, account.Role.Value);
    }
}