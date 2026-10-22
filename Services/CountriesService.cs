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

    public List<CountryResponse> GetAllCountries()
    {
        throw new NotImplementedException();
    }
}

//Check i countryAddRequest is not null
//Validate all properties of countryAddRequest
//Convert countryAddRequest from CountryAddRequest type to Country
//Generate a new CountryId
//Then add it into List<Country>
//Return CountryResponse object with generated CountryId