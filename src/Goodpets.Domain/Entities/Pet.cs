namespace Goodpets.Domain.Entities;

public sealed class Pet : Entity
{
    public PetId PetId { get; private set; }
    public string Name { get; private set; }
    public Owner Owner { get; private set; }
    public LocalDate DateOfBirth { get; private set; }
    public float Weight { get; private set; }
    public string Gender { get; private set; }
    public string Breed { get; private set; }
    public string Coat { get; private set; }

    private Pet()
    {
        PetId = null!;
        Name = null!;
        Owner = null!;
        Weight = 0.0F;
        Gender = null!;
        Breed = null!;
        Coat = null!;
    }

    public static Result<Pet> Create(string name, LocalDate dateOfBirth, float weight, string gender, string breed,
        string coat,
        Owner owner)
    {
        var pet = new Pet();

        var resultName = pet.ChangeName(name);
        var resultDate = pet.ChangeDateOfBirth(dateOfBirth);
        var resultWeight = pet.ChangeWeight(weight);
        var resultGender = pet.ChangeGender(gender);
        var resultBreed = pet.ChangeBreed(breed);
        var resultCoat = pet.ChangeCoat(coat);
        var resultOwner = pet.ChangeOwner(owner);

        var mergedResult = Result.Merge(resultName, resultDate, resultWeight, resultGender, resultBreed, resultCoat,
            resultOwner);

        return mergedResult.IsFailed ? mergedResult : Result.Ok(pet);
    }

    public Result ChangeOwner(Owner? owner)
    {
        if (owner is null)
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(owner)));

        Owner = owner;

        return Result.Ok();
    }

    public Result ChangeCoat(string coat)
    {
        if (string.IsNullOrEmpty(coat))
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(coat)));

        Coat = coat;

        return Result.Ok();
    }

    public Result ChangeBreed(string breed)
    {
        if (string.IsNullOrEmpty(breed))
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(breed)));

        Breed = breed;

        return Result.Ok();
    }

    public Result ChangeGender(string gender)
    {
        if (string.IsNullOrEmpty(gender))
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(nameof(gender)));

        Gender = gender;

        return Result.Ok();
    }

    public Result ChangeWeight(float weight)
    {
        if (weight <= 0.0F)
            return Result.Fail(
                new Error("Pet can't weight less or equals to 0").WithErrorCode(nameof(weight)));

        Weight = weight;

        return Result.Ok();
    }

    public Result ChangeDateOfBirth(LocalDate dateOfBirth)
    {
        if (SystemClock.Instance.InUtc().GetCurrentDate().Year - dateOfBirth.Year > 30)
        {
            return Result.Fail(new Error("Invalid date of birth of your pet").WithErrorCode(nameof(dateOfBirth)));
        }

        DateOfBirth = dateOfBirth;

        return Result.Ok();
    }

    public Result ChangeName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return Result.Fail(ErrorResultMessages.NotNullOrEmptyError(name));

        Name = name;

        return Result.Ok();
    }
}