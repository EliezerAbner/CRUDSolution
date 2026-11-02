using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
/// Person domain model class
/// </summary>
public class Person
{
    [Key]
    public Guid PersonId { get; set; }
    
    [StringLength(40)]
    public string? PersonName { get; set; }
    
    [StringLength(40)]
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    [StringLength(10)]
    public string? Gender { get; set; }
    public Guid? CountryId { get; set; }
    
    [StringLength(200)]
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    [StringLength(80)]
    public string? Tin { get; set; } //TIN means Tax Identification Number (basically the CPF, in Brazil or SSN in US)
    
}