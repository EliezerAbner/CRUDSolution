using System.ComponentModel.DataAnnotations;
using ServiceContracts.DTO;
using ServiceContracts;
using Entities;
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
        /*
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(searchBy))
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(Person.PersonName):
                matchingPersons = allPersons.Where(temp => (string.IsNullOrEmpty(temp.PersonName)? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true);
                break;
            
            case nameof(Person.Email):
                
        }
        */
        throw new NotImplementedException();
    }
}