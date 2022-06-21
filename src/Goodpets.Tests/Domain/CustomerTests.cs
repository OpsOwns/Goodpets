
namespace Goodpets.Tests.Domain;

public class CustomerTests
{
    [Fact]
    public void Customer_can_be_registered()
    {
        var customer = CreateCustomer();

        Assert.NotNull(customer);
    }

    [Fact]
    public void Customer_can_not_change_contact_email_with_wrong_email_address()
    {
        var faker = new Faker();

        var email = Email.Create(faker.Internet.Email().Substring(0, 5));

        Assert.True(email.IsFailed);
        Assert.Equal("Email is invalid", email.Errors[0].Message);

        var customer = CreateCustomer();

        Assert.Throws<BusinessException>(() => customer.ChangeContactEmail(email.ValueOrDefault));
        Assert.Throws<InvalidOperationException>(() => customer.ChangeContactEmail(email.Value));
    }

    [Fact]
    public void Customer_can_assign_pet_to_yourself()
    {
        var customer = CreateCustomer();

        var pet = CreatePet(customer);

        customer.AssignPet(pet);

        Assert.Equal(1, customer.Pets.Count);
    }

    [Fact]
    public void Customer_cant_assign_same_pet_twice()
    {
        var customer = CreateCustomer();

        var pet = CreatePet(customer);

        customer.AssignPet(pet);

        Assert.Throws<BusinessException>(() => customer.AssignPet(pet));
    }

    private static Pet CreatePet(Customer customer)
    {
        var pet = Pet.Create("Doris", new DateTime(2016, 3, 12), 5.0F, "Female", "Cat", "Brown", customer);
        return pet;
    }

    private static Customer CreateCustomer()
    {
        var faker = new Faker();

        var email = Email.Create(faker.Internet.Email());
        var address = Address.Create(faker.Address.City(), faker.Address.StreetAddress(), faker.Address.ZipCode());
        var fullName = FullName.Create(faker.Name.FirstName(), faker.Name.LastName());
        var phoneNumber = PhoneNumber.Create(faker.Random.Number(100000000, 999999999).ToString());

        var customer = Customer.Register(new UserId(), email.Value, address.Value, fullName.Value, phoneNumber.Value);

        return customer;
    }
}