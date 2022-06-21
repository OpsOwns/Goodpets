namespace Goodpets.Domain.Entities;

public sealed class Pet : Entity
{
    public PetId PetId { get; private set; }
    public string Name { get; private set; }
    public Customer Customer { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public float Weight { get; private set; }
    public string Gender { get; private set; }
    public string Breed { get; private set; }
    public string Coat { get; private set; }

    private Pet()
    {
        PetId = null!;
        Name = null!;
        Customer = null!;
        Weight = 0.0F;
        Gender = null!;
        Breed = null!;
        Coat = null!;
    }

    public static Pet Create(string name, DateTime dateOfBirth, float weight, string gender, string breed, string coat,
        Customer owner)
    {
        var pet = new Pet();

        pet.ChangeName(name);
        pet.ChangeDateOfBirth(dateOfBirth);
        pet.ChangeWeight(weight);
        pet.ChangeGender(gender);
        pet.ChangeBreed(breed);
        pet.ChangeCoat(coat);
        pet.ChangeOwner(owner);

        return pet;
    }

    public void ChangeOwner(Customer customer)
    {
        Customer = customer ?? throw new BusinessException("Customer can't be null");
    }

    public void ChangeCoat(string coat)
    {
        if (string.IsNullOrEmpty(coat))
            throw new BusinessException("Pet coat can't be null or empty");

        Coat = coat;
    }

    public void ChangeBreed(string breed)
    {
        if (string.IsNullOrEmpty(breed))
            throw new BusinessException("Pet breed can't be null or empty");

        Breed = breed;
    }

    public void ChangeGender(string gender)
    {
        if (string.IsNullOrEmpty(gender))
            throw new BusinessException("Pet gender can't be null or empty");

        Gender = gender;
    }

    public void ChangeWeight(float weight)
    {
        if (weight <= 0.0F)
            throw new BusinessException("Pet can't weight less or equals to 0");

        Weight = weight;
    }

    public void ChangeDateOfBirth(DateTime dateOfBirth)
    {
        if (DateTime.UtcNow.Year - dateOfBirth.Date.Year > 30)
        {
            throw new BusinessException("Invalid date of birth of your pet");
        }

        DateOfBirth = dateOfBirth;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new BusinessException("Pet name can't be null or empty");

        Name = name;
    }
}