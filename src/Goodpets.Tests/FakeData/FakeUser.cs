namespace Goodpets.Tests.FakeData;

public static class FakeUser
{
    public static User CreateFakeUser()
    {
        var faker = new Faker();

        var user = User.Create(Role.User(), Username.Create(faker.Internet.UserName()).Value,
            Password.Create(faker.Internet.Password()).Value,
            Email.Create(faker.Internet.Email()).Value).Value;

        user.ChangeToken(Token.Create(faker.Random.Word(), LocalDateTime.MaxIsoValue, false, new JwtId()).Value);

        return user;
    }
}