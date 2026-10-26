using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// Represents the DTO class that is used as return most methods of Persons Service
/// </summary>
public class PersonResponse
{
    public Guid PersonId { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? Address { get; set; } 
    public bool ReceiveNewsLetters { get; set; }
    public double? Age { get; set; }

    /// <summary>
    /// Compares the current object data with the parameter object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>True or false, indicating whether all person details are matched with the specified parameter</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj.GetType() != typeof(PersonResponse)) return false;

        PersonResponse person = (PersonResponse)obj;
        return PersonId == person.PersonId && PersonName == person.PersonName && Email == person.Email && DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryId == person.CountryId && Address == person.Address && ReceiveNewsLetters == person.ReceiveNewsLetters;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
    public override string ToString()
    {
        return $"Person Id: {PersonId}, Person Name: {PersonName}, Email: {Email}, Date Of Birth: {DateOfBirth?.ToString("yyyy MM dd")}, Gender: {Gender}, CountryId: {CountryId}, Address: {Address}, Receive News Letters: {ReceiveNewsLetters}";
    }

    public PersonUpdateRequest ToPersonUpdateRequest()
    {
        return new PersonUpdateRequest()
        {
            PersonId = PersonId,
            PersonName = PersonName,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
            Address = Address,
            ReceiveNewsLetters = ReceiveNewsLetters,
        };
    }
}

public static class PersonExtensions
{
    /// <summary>
    /// An extension method to convert an object of Person class into PersonResponse class
    /// </summary>
    /// <param name="person">The Person object to convert</param>
    /// <returns>Returns the converted PersonResponse object</returns>
    public static PersonResponse ToPersonResponse(this Person person)
    {
        //person => PersonResponse
        return new PersonResponse()
        {
            PersonId = person.PersonId,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            CountryId = person.CountryId,
            Address = person.Address,
            ReceiveNewsLetters = person.ReceiveNewsLetters,
            Age = (person.DateOfBirth != null)? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
        };
    }
}