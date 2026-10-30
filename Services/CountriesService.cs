using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService(bool initialize = true)
    {
        _countries = new List<Country>();
        
        if (initialize)
        {
            _countries.AddRange(new List<Country>
            {
                new Country() {CountryId = Guid.Parse("0429DF97-173A-43AC-BC17-02F058C776E0"), CountryName = "United Kingdom"},
                new Country() {CountryId = Guid.Parse("28067E80-52A1-405A-8F2B-367C93BA4A63"), CountryName = "Canada"},
                new Country() {CountryId = Guid.Parse("2739A752-29E8-4A93-9A07-53211148F9AE"), CountryName = "Chile"},
                new Country() {CountryId = Guid.Parse("53CDF383-EEAE-42A4-8CF2-9B82C18A42E2"), CountryName = "New Zealand"},
                new Country() {CountryId = Guid.Parse("6402C463-05FF-4E91-8C87-DE59CA688A3A"), CountryName = "Brazil"},
            });
        }
    }
    
    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        //validation: countryAddRequest parameter can't be null
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }
        
        //Validation: CountryName can't be null
        if (countryAddRequest.CountryName == null)
        {
            throw new ArgumentException(countryAddRequest.CountryName);
        }
        
        //Validation: CountryName can't be duplicate
        if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
        {
            throw new ArgumentException("Country already exists");
        }
        
        //Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry();
        country.CountryId = Guid.NewGuid();
        
        _countries.Add(country);
        
        return country.ToCountryResponse();
    }

    //Converts from Country object to CountryResponse object
    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null)
            return null;
                
        Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryId == countryId);
        
        if(country_response_from_list == null)
            return null;
        
        return country_response_from_list?.ToCountryResponse();
    }
}

//Check if countryAddRequest is not null
//Validate all properties of countryAddRequest
//Convert countryAddRequest from CountryAddRequest type to Country
//Generate a new CountryId
//Then add it into List<Country>
//Return CountryResponse object with generated CountryId