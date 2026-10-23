using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService()
    {
        _countries = new List<Country>();
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