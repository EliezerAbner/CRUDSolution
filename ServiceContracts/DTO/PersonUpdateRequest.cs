using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// Represents the DTO class that contains the person details to update
/// </summary>
public class PersonUpdateRequest
{
    [Required(ErrorMessage = "Person Id can't be blank")]
    public Guid PersonId { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    public string? PersonName { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public GenderOptions? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? Address { get; set; } 
    public bool ReceiveNewsLetters { get; set; }

    /// <summary>
    /// Converts the current object of PersonUpdateRequest into a new object of Person type
    /// </summary>
    /// <returns>Returns Person object</returns>
    public Person ToPerson()
    {
        return new Person()
        {
            PersonId = PersonId,
            PersonName = PersonName, 
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Gender.ToString(),
            Address = Address,
            CountryId = CountryId,
            ReceiveNewsLetters = ReceiveNewsLetters
        };
    }
}