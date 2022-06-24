namespace Goodpets.Tests.Domain;

public class OwnerTests
{
    [Fact]
    public void Owner_can_be_registered()
    {
        var customer = CreateOwner();

        customer.ShouldNotBeNull();
    }

    [Fact]
    public void Owner_can_not_change_contact_email_with_wrong_email_address()
    {
        var faker = new Faker();

        var email = Email.Create(faker.Internet.Email().Substring(0, 5));

        email.IsFailed.ShouldBe(true);
        email.Errors[0].Message.ShouldBe("Email is invalid");

        var owner = CreateOwner();

        owner.ChangeContactEmail(email.ValueOrDefault).IsFailed.ShouldBeTrue();
    }

    [Fact]
    public void Owner_can_assign_pet_to_yourself()
    {
        var owner = CreateOwner();

        var pet = CreatePet(owner);

        owner.AssignPet(pet);

        owner.Pets.Count.ShouldBeEquivalentTo(1);
    }

    [Fact]
    public void Owner_can_not_assign_not_his_pet_to_yourself()
    {
        var owner = CreateOwner();

        var pet = CreatePet(owner);

        var owner2 = CreateOwner();

        var result = owner2.AssignPet(pet);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].Message.ShouldBeEquivalentTo("This pet has different owner");
    }

    [Fact]
    public void Owner_cant_assign_same_pet_twice()
    {
        var owner = CreateOwner();

        var pet = CreatePet(owner);

        owner.AssignPet(pet);

        owner.AssignPet(pet).IsFailed.ShouldBeTrue();
    }

    private static Pet CreatePet(Owner customer)
    {
        var pet = Pet.Create("Doris", new LocalDate(2016, 3, 12), 5.0F, "Female", "Cat", "Brown", customer);
        return pet.Value;
    }

    private static Owner CreateOwner()
    {
        var faker = new Faker();

        var email = Email.Create(faker.Internet.Email());
        var address = Address.Create(faker.Address.City(), faker.Address.StreetAddress(), faker.Address.ZipCode());
        var fullName = FullName.Create(faker.Name.FirstName(), faker.Name.LastName());
        var phoneNumber = PhoneNumber.Create(faker.Random.Number(100000000, 999999999).ToString());
        var owner = Owner.Register(new UserId(), email.Value, address.Value, fullName.Value, phoneNumber.Value);

        return owner.Value;
    }
}