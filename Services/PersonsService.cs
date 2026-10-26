using System.ComponentModel.DataAnnotations;
using ServiceContracts.DTO;
using ServiceContracts;
using Entities;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonsService
{
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;

    public PersonsService()
    {
        _persons = new List<Person>();
        _countriesService = new CountriesService();
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();
        personResponse.CountryName = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
        
        return personResponse;
    }
    
    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
        //check if PersonAddRequest is not null
        if (personAddRequest == null)
        {
            throw new ArgumentNullException(nameof(personAddRequest));
        }
        
        //Model validations
        ValidationHelper.ModelValidation(personAddRequest);
        
        //convert personAddRequest into Person type
        Person person = personAddRequest.ToPerson();
        
        //generate PersonId
        person.PersonId = Guid.NewGuid();
        
        //add person object to persons list
        _persons.Add(person);
        
        //convert the Person object into PersonResponse type
        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(temp => temp.ToPersonResponse()).ToList();
    }

    public PersonResponse? GetPersonByPersonId(Guid? personId)
    {  
        if (personId == null)
        {
            return null;
        }

        Person? person = _persons.FirstOrDefault(temp => temp.PersonId == personId);
        
        if (person == null)
        {
            return null;
        }
        
        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(Person.PersonName):
                matchingPersons = allPersons.Where(temp => (string.IsNullOrEmpty(temp.PersonName)? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            case nameof(Person.Email):
                matchingPersons = allPersons.Where(temp => (string.IsNullOrEmpty(temp.Email)? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
           
            case nameof(Person.DateOfBirth):
                matchingPersons = allPersons.Where(temp => (temp.DateOfBirth != null)? temp.DateOfBirth.Value.ToString("yyyy MM dd").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;
            
            case nameof(Person.Gender):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender)? temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            case nameof(Person.CountryId):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.CountryName)? temp.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            case nameof(Person.Address):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address)? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            default: matchingPersons = allPersons;
                break;
        }
        
        return matchingPersons;
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy))
            return allPersons;
        
        //switch expression
        List<PersonResponse> sortedPersons = (sortBy, sortOrder)
            switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),
                
                (nameof(PersonResponse.Age), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.Age).ToList(),
                
                (nameof(PersonResponse.Gender), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                
                (nameof(PersonResponse.CountryName), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),
                
                (nameof(PersonResponse.Address), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.Asc)
                    => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.Desc)
                    => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),
                
                _ => allPersons
            };
        
        return sortedPersons;
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest == null)
            throw new ArgumentNullException(nameof(Person));
        
        ValidationHelper.ModelValidation(personUpdateRequest);

        Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonId == personUpdateRequest.PersonId);
        
        if (matchingPerson == null)
            throw new ArgumentException("Given person id doesn't exist");

        matchingPerson.PersonName = personUpdateRequest.PersonName;
        matchingPerson.Email = personUpdateRequest.Email;
        matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        matchingPerson.CountryId = personUpdateRequest.CountryId;
        matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
        
        return matchingPerson.ToPersonResponse();
    }

    public bool DeletePerson(Guid? personId)
    {
        if (personId == null)
            throw new ArgumentNullException(nameof(personId));

        Person? person = _persons.FirstOrDefault(temp => temp.PersonId == personId);

        if (person == null)
            return false;
        
        _persons.RemoveAll(temp => temp.PersonId == personId);
        return true;
    }
}