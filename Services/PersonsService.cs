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

    public PersonsService(bool initialize = true)
    {
        _persons = new List<Person>();
        _countriesService = new CountriesService();

        if (initialize)
        {
            _persons.Add(new Person() // https://mockaroo.com/ for generating mock data
            {
                PersonId = Guid.Parse("68BD2C28-F70A-4A5A-A3A0-923D507249FA"),
                PersonName = "Fritz",
                CountryId = Guid.Parse("6402C463-05FF-4E91-8C87-DE59CA688A3A"),
                Email = "fchristy0@ustream.tv",
                DateOfBirth = DateTime.Parse("1996-05-19"),
                Gender = "Male",
                Address = "2437 Mcguire Pass",
                ReceiveNewsLetters = false
            });

            _persons.Add(new Person()
            {
                PersonId = Guid.Parse("C9CD8200-C7BF-4870-A119-6031C236F1E4"),
                PersonName = "Elfrida",
                CountryId = Guid.Parse("53CDF383-EEAE-42A4-8CF2-9B82C18A42E2"),
                Email = "eyashin1@engadget.com",
                DateOfBirth = DateTime.Parse("1990-07-18"),
                Gender = "Female",
                Address = "8127 Gina Way",
                ReceiveNewsLetters = true
            });
                
            _persons.Add(new Person()
            {
                PersonId  = Guid.Parse("5B45E303-71B0-45FE-AFF3-EB2B06D95891"),
                PersonName = "Dar",
                CountryId = Guid.Parse("2739A752-29E8-4A93-9A07-53211148F9AE"),
                Email = "dfarrant2@360.cn",
                DateOfBirth = DateTime.Parse("2001-01-01"),
                Gender = "Male",
                Address = "32 Sachs Alley",
                ReceiveNewsLetters = false 
            });
                
            _persons.Add(new Person()
            {
                PersonId  = Guid.Parse("68E82485-8AF0-4B6B-A6EA-57DA11392DD2"),
                PersonName = "Cristi",
                CountryId = Guid.Parse("28067E80-52A1-405A-8F2B-367C93BA4A63"),
                Email = "carden3@opensource.org",
                DateOfBirth = DateTime.Parse("1990-08-01"),
                Gender = "Female",
                Address = "505 Katie Circle",
                ReceiveNewsLetters = false 
            });
                
            _persons.Add(new Person()
            {
                PersonId  = Guid.Parse("C0ED53B1-C23A-44A6-8B69-775BB65ACF90"),
                PersonName = "Hi",
                CountryId = Guid.Parse("0429DF97-173A-43AC-BC17-02F058C776E0"),
                Email = "hphinnessy8@imgur.com",
                DateOfBirth = DateTime.Parse("1999-03-25"),
                Gender = "Male",
                Address = "8127 Gina Way",
                ReceiveNewsLetters = true 
            });
                
            _persons.Add(new Person()
            {
                PersonId  = Guid.Parse("BFA4530D-A63D-485D-A44A-5FED4E36ABFD"),
                PersonName = "Harlene",
                CountryId = Guid.Parse("6402C463-05FF-4E91-8C87-DE59CA688A3A"),
                Email = "hleathem9@liveinternet.ru",
                DateOfBirth = DateTime.Parse("1993-11-23"),
                Gender = "Female",
                Address = "12424 Waxwing Court",
                ReceiveNewsLetters = true 
            });
            
            //_countriesService.
        }
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
            case nameof(PersonResponse.PersonName):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName)? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            case nameof(PersonResponse.Email):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email)? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
           
            case nameof(PersonResponse.DateOfBirth):
                matchingPersons = allPersons.Where(temp => (temp.DateOfBirth != null)? temp.DateOfBirth.Value.ToString("yyyy MM dd").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;
            
            case nameof(PersonResponse.Gender):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender)? temp.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            case nameof(PersonResponse.CountryId):
                matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.CountryName)? temp.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            case nameof(PersonResponse.Address):
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

