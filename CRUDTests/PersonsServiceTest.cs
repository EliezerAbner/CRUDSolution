using Xunit;
using Entities;
using Services;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Xunit.Abstractions;


namespace CRUDTests;

public class PersonsServiceTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _personsService = new PersonsService();
        _countriesService = new CountriesService();
        _testOutputHelper = testOutputHelper;
    }
    
    #region AddPerson
    
    //When we supply null value as PersonName, it should throw ArgumentException
    [Fact]
    public void AddPerson_PersonNameIsNull()
    {
        //Arrange
        PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null }; 
        
        //Act
        Assert.Throws<ArgumentException>(() =>
        {
            _personsService.AddPerson(personAddRequest);
        });
    }
    
    //When we supply proper person details, it should insert the person into the persons list and it should return
    // an object of PersonResponse, which includes with the newly generated person id
    [Fact]
    public void AddPerson_ProperPersonDetails()
    {
        //Arrange
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = "Person name",
            Email = "person@email.com",
            Address = "Person address",
            CountryId = Guid.NewGuid(),
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2000-05-14"),
        }; 
        
        //Act
        PersonResponse person_response_from_add = _personsService.AddPerson(personAddRequest);
        List<PersonResponse>? person_list = _personsService.GetAllPersons();

        //Assert
        Assert.True(person_response_from_add.PersonId != Guid.Empty);
        Assert.Contains(person_response_from_add, person_list);
    }
    #endregion
    
    #region GetPersonPersonId
    
    //If we supply null as PersonId, it should return null as PersonResponse
    [Fact]
    public void GetPersonId_NullPersonId()
    {
        //Arrange
        Guid? personId = null;
        
        //Act
        PersonResponse? person_response_from_get = _personsService.GetPersonByPersonId(personId);
        
        //Assert
        Assert.Null(person_response_from_get);
    }

    //If we supply a valid person Id, it should return the valid person details as PersonResponse object
    [Fact]
    public void GetPersonIdByPersonId_WithPersonId()
    {
        //Arrange
        CountryAddRequest country_request = new CountryAddRequest() {CountryName = "France"};
        CountryResponse country_response = _countriesService.AddCountry(country_request);
        
        //Act
        PersonAddRequest? personAddRequest = new PersonAddRequest()
        {
            PersonName = "Person name",
            Email = "person@email.com",
            Address = "Person address",
            CountryId = country_response.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-05-14"),
        }; 
        PersonResponse person_response_from_add = _personsService.AddPerson(personAddRequest);
        
        //Assert
        PersonResponse? person_response_from_get = _personsService.GetPersonByPersonId(person_response_from_add.PersonId);
        
        Assert.Equal(person_response_from_add, person_response_from_get);
    }
    
    #endregion
    
    #region GetAllPersons
    
    //The GetAllPersons() should return an empty list by default
    [Fact]
    public void GetAllPersons_EmptyList()
    {
        //Act
        List<PersonResponse> persons_from_get = _personsService.GetAllPersons();
        
        //Assert
        Assert.Empty(persons_from_get);
    }
    
    // First, we will add few persons; and then we call GetAllPersons(), it should return the same persons that were added
    [Fact]
    public void GetAllPersons_AddFewPersons()
    {
        //Arrange
        CountryAddRequest country_request_1 =  new CountryAddRequest() {CountryName = "Chile"};
        CountryAddRequest country_request_2 = new CountryAddRequest() {CountryName = "Canada"};
        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);
        
        PersonAddRequest? person_request_1 = new PersonAddRequest()
        {
            PersonName = "Person name",
            Email = "person@email.com",
            Address = "Person address",
            CountryId = country_response_1.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-05-14"),
        };
        
        PersonAddRequest? person_request_2 = new PersonAddRequest()
        {
            PersonName = "Person name 2",
            Email = "person2@email.com",
            Address = "Person address",
            CountryId = country_response_2.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2001-02-19"),
        };
        
        PersonAddRequest? person_request_3 = new PersonAddRequest()
        {
            PersonName = "Person name 3",
            Email = "person3@email.com",
            Address = "Person address",
            CountryId = country_response_2.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2009-12-19"),
        };
        
        List<PersonAddRequest> person_requests = new List<PersonAddRequest>(){person_request_1, person_request_2, person_request_3};
        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }
        
        //print person_response_list_from_add
        _testOutputHelper.WriteLine("Expected: ");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }
        
        //Act
        List<PersonResponse> persons_list_from_get = _personsService.GetAllPersons();
        
        //print person_response_list_from_get
        _testOutputHelper.WriteLine("Actual: ");
        foreach (PersonResponse person_response_from_get in persons_list_from_get)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }
        
        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            Assert.Contains(person_response_from_add, persons_list_from_get);
        }
    }
    
    #endregion
    
    #region GetFilteredPersons
    
    //If the search text is empty and search by is "PersonName", it should return all persons
    [Fact]
    public void GetFilteredPersons_EmptySearchText()
    {
        //Arrange
        CountryAddRequest country_request_1 =  new CountryAddRequest() {CountryName = "Chile"};
        CountryAddRequest country_request_2 = new CountryAddRequest() {CountryName = "Canada"};
        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);
        
        PersonAddRequest? person_request_1 = new PersonAddRequest()
        {
            PersonName = "Person name",
            Email = "person@email.com",
            Address = "Person address",
            CountryId = country_response_1.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-05-14"),
        };
        
        PersonAddRequest? person_request_2 = new PersonAddRequest()
        {
            PersonName = "Person name 2",
            Email = "person2@email.com",
            Address = "Person address",
            CountryId = country_response_2.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2001-02-19"),
        };
        
        PersonAddRequest? person_request_3 = new PersonAddRequest()
        {
            PersonName = "Person name 3",
            Email = "person3@email.com",
            Address = "Person address",
            CountryId = country_response_2.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2009-12-19"),
        };
        
        List<PersonAddRequest> person_requests = new List<PersonAddRequest>(){person_request_1, person_request_2, person_request_3};
        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }
        
        //Act
        List<PersonResponse> persons_list_from_get = _personsService.GetFilteredPersons(nameof(Person.PersonName), "");
        
        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            Assert.Contains(person_response_from_add, persons_list_from_get);
        }
    }
    
    //First we will add few persons; and then we will search based on person name with some search string.
    //It must return the matching persons
    [Fact]
    public void GetFilteredPersons_SearchByPersonName()
    {
        //Arrange
        CountryAddRequest country_request_1 =  new CountryAddRequest() {CountryName = "Chile"};
        CountryAddRequest country_request_2 = new CountryAddRequest() {CountryName = "Canada"};
        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);
        
        PersonAddRequest? person_request_1 = new PersonAddRequest()
        {
            PersonName = "Smith",
            Email = "person@email.com",
            Address = "Person address",
            CountryId = country_response_1.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-05-14"),
        };
        
        PersonAddRequest? person_request_2 = new PersonAddRequest()
        {
            PersonName = "Mary",
            Email = "person2@email.com",
            Address = "Person address",
            CountryId = country_response_2.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2001-02-19"),
        };
        
        PersonAddRequest? person_request_3 = new PersonAddRequest()
        {
            PersonName = "Rahman",
            Email = "person3@email.com",
            Address = "Person address",
            CountryId = country_response_2.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2009-12-19"),
        };
        
        List<PersonAddRequest> person_requests = new List<PersonAddRequest>(){person_request_1, person_request_2, person_request_3};
        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }
        
        //Act
        List<PersonResponse> persons_list_from_get = _personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");
        
        //Assert
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            if (person_response_from_add.PersonName != null)
            {
                if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(person_response_from_add, persons_list_from_get);
                }
            }
        }
    }
    #endregion
    
    #region GetSortedPersons
    
    //When we sort based on PersonName in Desc, it should return persons list in descending on PersonName
    [Fact]
    public void GetSortedPersons()
    {
        //Arrange
        CountryAddRequest country_request_1 =  new CountryAddRequest() {CountryName = "Uruguay"};
        CountryAddRequest country_request_2 = new CountryAddRequest() {CountryName = "New Zealand"};
        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);
        
        PersonAddRequest? person_request_1 = new PersonAddRequest()
        {
            PersonName = "Smith",
            Email = "person@email.com",
            Address = "Person address",
            CountryId = country_response_1.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2005-05-14"),
        };
        
        PersonAddRequest? person_request_2 = new PersonAddRequest()
        {
            PersonName = "Mary",
            Email = "person2@email.com",
            Address = "Person address",
            CountryId = country_response_2.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2001-02-19"),
        };
        
        PersonAddRequest? person_request_3 = new PersonAddRequest()
        {
            PersonName = "Rahman",
            Email = "person3@email.com",
            Address = "Person address",
            CountryId = country_response_2.CountryId,
            Gender = GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2009-12-19"),
        };
        
        List<PersonAddRequest> person_requests = new List<PersonAddRequest>(){person_request_1, person_request_2, person_request_3};
        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }
        
        List<PersonResponse> allPersons = _personsService.GetAllPersons();
        
        //Act
        List<PersonResponse> persons_list_from_sort = _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.Desc);
        
        person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();
        
        //Assert
        for (int i = 0; i < person_response_list_from_add.Count; i++)
        {
            Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
        }
    }
    
    #endregion
    
    #region UpdatePerson
    
    //When we supply null as PersonUpdateRequest, it should throw ArgumentNullExceptiion
    [Fact]
    public void UpdatePerson_NullPerson()
    {
        PersonUpdateRequest? person_update_request = null;
        Assert.Throws<ArgumentNullException>(() => _personsService.UpdatePerson(person_update_request));
    }
    
    //When we supply invalid person id, it should throw ArgumentException
    [Fact]
    public void UpdatePerson_InvalidPersonId()
    {
        PersonUpdateRequest? person_update_request = new PersonUpdateRequest() {  PersonId = Guid.NewGuid() };
        
        Assert.Throws<ArgumentException>(() => _personsService.UpdatePerson(person_update_request));
    }
    
    //When the person name is null, it should throw ArgumentException
    [Fact]
    public void UpdatePerson_PersonNameIsNull()
    {
        CountryAddRequest country_add_request = new CountryAddRequest() {CountryName = "Uruguay"};
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

        PersonAddRequest person_add_request = new PersonAddRequest()
        {
            PersonName = "Mary",
            Email = "mary@email.com",
            Gender =  GenderOptions.Female,
            CountryId = country_response_from_add.CountryId,
        };
        PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);
        
        PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
        person_update_request.PersonName = null;
        
        Assert.Throws<ArgumentException>(() => _personsService.UpdatePerson(person_update_request));
    }
    
    //First, add a new person and try to update the person name and email
    [Fact]
    public void UpdatePerson_PersonFullDetailsUpdate()
    {
        CountryAddRequest country_add_request = new CountryAddRequest() {CountryName = "Uruguay"};
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

        PersonAddRequest person_add_request = new PersonAddRequest()
        {
            PersonName = "Mary",
            CountryId = country_response_from_add.CountryId,
            Address = "Person address",
            DateOfBirth = DateTime.Parse("2002-02-19"),
            Email = "person@email.com",
            Gender = GenderOptions.Female,
        };
        PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);
        
        PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
        person_update_request.PersonName = "Suzan";
        person_update_request.Email = "suzan@email.com";
        
        PersonResponse person_response_from_update = _personsService.UpdatePerson(person_update_request);
        PersonResponse? person_response_from_get = _personsService.GetPersonByPersonId(person_response_from_update.PersonId);
        
        Assert.Equal(person_response_from_get, person_response_from_update);
    }
    
    #endregion
    
    #region DeletePerson
    
    //If you supply a valid PersonId, it should return true
    [Fact]
    public void DeletePerson_ValidPersonId()
    {
        CountryAddRequest country_add_request = new CountryAddRequest() {CountryName = "Jamaica"};
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);
        
        PersonAddRequest? person_add_request = new PersonAddRequest()
        {
            PersonName = "Person name",
            Email = "person@email.com",
            Address = "Person address",
            CountryId = country_response_from_add.CountryId,
            Gender = GenderOptions.Other,
            DateOfBirth = DateTime.Parse("2000-05-14"),
            ReceiveNewsLetters =  false,
        }; 
        PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);
        
        bool isDeleted = _personsService.DeletePerson(person_response_from_add.PersonId);
        Assert.True(isDeleted);
    }
    
    //If you supply a invalid PersonId, it should return false
    [Fact]
    public void DeletePerson_InvalidPersonId()
    {
        bool isDeleted = _personsService.DeletePerson(Guid.NewGuid());
        Assert.False(isDeleted);
    }
    
    #endregion
}


/*
 * Different types of exceptions
 *
 * NullReferenceException   -> You try to access a member of an object that is null.
 * ArgumentException        -> A method receives an invalid argument.
 * ArgumentNullException    -> A method receives null when it doesn't accept it. 
 */
