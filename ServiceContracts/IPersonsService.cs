using ServiceContracts.DTO;
using ServiceContracts.Enums;


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
    Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
    
    /// <summary>
    /// Returns all Persons
    /// </summary>
    /// <returns>Returns a list of objects of PersonResponse</returns>
    Task<List<PersonResponse>> GetAllPersons();
    
    /// <summary>
    /// Return the person object based on the given person Id
    /// </summary>
    /// <param name="personId">Person id to search</param>
    /// <returns>Returns matching person object</returns>
    Task<PersonResponse?> GetPersonByPersonId(Guid? personId);

    /// <summary>
    /// Returns all persons objects that matches with the given search field and search string
    /// </summary>
    /// <param name="searchBy">Search field to search</param>
    /// <param name="searchString">Search string to search</param>
    /// <returns>Returns all matching persons based on the given search field and search string</returns>
    Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

    /// <summary>
    /// Returns sorted list of persons
    /// </summary>
    /// <param name="allPersons">Represents list of persons to sort</param>
    /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted</param>
    /// <param name="sortOrder">Asc or Desc</param>
    /// <returns>Returns the sorted persons as PersonResponse list</returns>
    Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
    
    /// <summary>
    /// Updates the specified person details based on the given person Id
    /// </summary>
    /// <param name="personUpdateRequest">Person details to update, including person Id</param>
    /// <returns>Returns the person response object</returns>
    Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    
    /// <summary>
    /// Deletes a person based on the given person id
    /// </summary>
    /// <param name="personId">PersonId to delete</param>
    /// <returns>Returns true, if the deletion is sucessful; otherwise false</returns>
    Task<bool> DeletePerson(Guid? personId);

    /// <summary>
    /// Returns persons as CSV
    /// </summary>
    /// <returns></returns>
    Task<MemoryStream> GetPersonCSV();
}