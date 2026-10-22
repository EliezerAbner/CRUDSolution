using System.Diagnostics.CodeAnalysis;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;
    
    public CountriesServiceTest()
    {
        _countriesService = new CountriesService();
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
        //Assert
        Assert.True(response.CountryId != Guid.Empty);
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
}