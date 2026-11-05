using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CsvHelper;
using ServiceContracts.DTO;
using ServiceContracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services; // https://mockaroo.com/ for generating mock data

public class PersonsService : IPersonsService
{
    private readonly PersonsDbContext _db;
    private readonly ICountriesService _countriesService;

    public PersonsService(PersonsDbContext personsDbContext, ICountriesService countriesService)
    {
        _db = personsDbContext;
        _countriesService = countriesService;
    }
    
    public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
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
        
        //add person object database
        _db.sp_InsertPerson(person);
        await _db.SaveChangesAsync();
        
        //convert the Person object into PersonResponse type
        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetAllPersons()
    {
        var persons = await _db.Persons.Include("Country").ToListAsync();
        
        return persons
            .Select(temp => temp.ToPersonResponse())
            .ToList();
    }

    public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
    {  
        if (personId == null)
            return null;

        Person? person = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personId);
        
        if (person == null)
            return null;
        
        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
    {
        
        List<PersonResponse> allPersons = await GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(PersonResponse.PersonName):
                matchingPersons = allPersons.Where(temp => 
                    (!string.IsNullOrEmpty(temp.PersonName)? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            case nameof(PersonResponse.Email):
                matchingPersons = allPersons.Where(temp => 
                    (!string.IsNullOrEmpty(temp.Email)? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
           
            case nameof(PersonResponse.DateOfBirth):
                matchingPersons = allPersons.Where(temp => 
                    (temp.DateOfBirth != null)? temp.DateOfBirth.Value.ToString("yyyy MM dd").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;
            
            case nameof(PersonResponse.Gender):
                matchingPersons = allPersons.Where(temp => 
                    (!string.IsNullOrEmpty(temp.Gender)? temp.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            case nameof(PersonResponse.CountryId):
                matchingPersons = allPersons.Where(temp => 
                    (!string.IsNullOrEmpty(temp.CountryName)? temp.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            case nameof(PersonResponse.Address):
                matchingPersons = allPersons.Where(temp => 
                    (!string.IsNullOrEmpty(temp.Address)? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;
            
            default: matchingPersons = allPersons;
                break;
        }
        
        return matchingPersons;
    }

    public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
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

    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest == null)
            throw new ArgumentNullException(nameof(Person));
        
        ValidationHelper.ModelValidation(personUpdateRequest);

        Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personUpdateRequest.PersonId);
        
        if (matchingPerson == null)
            throw new ArgumentException("Given person id doesn't exist");

        matchingPerson.PersonName = personUpdateRequest.PersonName;
        matchingPerson.Email = personUpdateRequest.Email;
        matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        matchingPerson.CountryId = personUpdateRequest.CountryId;
        matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
        
        await _db.SaveChangesAsync();
        return matchingPerson.ToPersonResponse();
    }

    public async Task<bool> DeletePerson(Guid? personId)
    {
        if (personId == null)
            throw new ArgumentNullException(nameof(personId));

        Person? person = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personId);

        if (person == null)
            return false;
        
        _db.Persons
            .Remove(_db.Persons.First (temp => temp.PersonId == personId));
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<MemoryStream> GetPersonCSV()
    {
        MemoryStream memoryStream = new MemoryStream();
        List<PersonResponse> persons = _db.Persons
            .Include("Country")
            .Select(temp => temp.ToPersonResponse()).ToList();
        
        await using (var writer = CsvWriter.Create(memoryStream, leaveOpen: true))
        {
            writer.WriteHeader<PersonResponse>();
            await writer.WriteRecordsAsync(persons);
        }

        memoryStream.Position = 0;
        return memoryStream;
    }
}

