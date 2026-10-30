using System.Diagnostics.CodeAnalysis;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;
    
    public CountriesServiceTest()
    {
        _countriesService = new CountriesService(false);
    }
    
    #region AddCountry
    //when CountryAddRequest is null, it should ArgumentNullException
    [Fact]
    public void AddCountry_NullCountry()
    {
        //Arrange
        CountryAddRequest? request = null;
        
        //Assert
        Assert.Throws<ArgumentNullException>(() => 
            //Act
            _countriesService.AddCountry(request)
        );
    }
    
    //when the CountryName is null, it should throw ArgumentException
    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        CountryAddRequest? request = new CountryAddRequest() { CountryName = null };
        Assert.Throws<ArgumentException>(() =>
        {
            _countriesService.AddCountry(request);
        });
    }
    
    //when the CountryName is duplicate, it should throw ArgumentException
    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
        CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "Brazil" };
        CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "Brazil" };
        
        Assert.Throws<ArgumentException>(() =>
        {
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);
        });
    }
    
    //when you supply proper country name, it should insert (add) the country to the existing list of countries
    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest() {CountryName = "Japan"};
        //Act
        CountryResponse response = _countriesService.AddCountry(request);
        List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();
        //Assert
        Assert.True(response.CountryId != Guid.Empty);
        Assert.Contains(response,countries_from_GetAllCountries);
    }
    #endregion
    
    #region GetAllCountries
    //The list of countries should be empty by default (before adding any countries)
    [Fact]
    public void GetAllCountries_EmptyList()
    {
        List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();
        Assert.Empty(actual_country_response_list);
        
    }

    [Fact]
    public void GetAllCountries_AddFewCountries()
    {
        //Arrange
        List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
        {
            new CountryAddRequest() {CountryName = "Japan"},
            new CountryAddRequest() {CountryName = "France"},
            new CountryAddRequest() {CountryName = "Spain"},
            new CountryAddRequest() {CountryName = "Brazil"},
        };

        //Act
        List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();
        
        foreach (CountryAddRequest country_request in country_request_list)
        {
            countries_list_from_add_country.Add(_countriesService.AddCountry(country_request));
        }
        
        List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();
        
        //read each element from countries_list_from_add_country
        foreach (CountryResponse expected_country in countries_list_from_add_country)
        {
            Assert.Contains(expected_country, actualCountryResponseList);
        }
    }
    #endregion
    
    #region GetCountryByCountryId

    //If we supply null as CountryId, it should return null as CountryResponse
    [Fact]
    public void GetCountryByCountryId_NullCountryId()
    {
        //Arrange
        Guid? countryId = null;
        
        //Act
        CountryResponse? country_response_from_get_method = _countriesService.GetCountryByCountryId(countryId);
        
        //Assert
        Assert.Null(country_response_from_get_method);
    }
    
    //If we supply a valid country id, it should return the matching country
    //details as CountryResponse object
    [Fact]
    public void GetCountryByCountryId_ValidCountryId()
    {
        //Arrange
        CountryAddRequest? country_add_request = new CountryAddRequest() {CountryName = "China"};
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);
        
        //Act
        CountryResponse? country_response_from_get = _countriesService.GetCountryByCountryId(country_response_from_add.CountryId);
        
        //Assert
        Assert.Equal(country_response_from_add, country_response_from_get);
    }
    
    #endregion
}