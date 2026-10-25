using ServiceContracts.DTO;


namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating Person entity
/// </summary>
public interface IPersonsService
{
    /// <summary>
    ///  Adds a new person into the list of persons
    /// </summary>
    /// <param name="personAddRequest">Person to add</param>
    /// <returns>Returns the same person details, along with newly generated PersonId</returns>
    PersonResponse AddPerson(PersonAddRequest? personAddRequest);
    
    /// <summary>
    /// Returns all Persons
    /// </summary>
    /// <returns>Returns a list of objects of PersonResponse</returns>
    List<PersonResponse> GetAllPersons();
    
    /// <summary>
    /// Return the person object based on the given person Id
    /// </summary>
    /// <param name="personId">Person id to search</param>
    /// <returns>Returns matching person object</returns>
    PersonResponse? GetPersonByPersonId(Guid? personId);

    /// <summary>
    /// Returns all persons objects that matches with the given search field and search string
    /// </summary>
    /// <param name="searchBy">Search field to search</param>
    /// <param name="searchString">Search string to search</param>
    /// <returns>Returns all matching persons based on the given search field and search string</returns>
    List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);
}