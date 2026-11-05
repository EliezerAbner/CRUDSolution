using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly PersonsDbContext _db;

    public CountriesService(PersonsDbContext personsDBContext)
    {
        _db = personsDBContext;
    }
    
    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
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
        if (await _db.Countries.CountAsync(temp => temp.CountryName == countryAddRequest.CountryName) > 0)
        {
            throw new ArgumentException("Country already exists");
        }
        
        //Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry();
        country.CountryId = Guid.NewGuid();
        
        _db.Countries.Add(country);
        await _db.SaveChangesAsync();
        
        return country.ToCountryResponse();
    }

    //Converts from Country object to CountryResponse object
    public async Task<List<CountryResponse>> GetAllCountries()
    {
        return await _db.Countries
            .Select(country => country.ToCountryResponse()).ToListAsync();
    }

    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null)
            return null;
                
        Country? countryResponse = await _db.Countries
            .FirstOrDefaultAsync(temp => temp.CountryId == countryId);
        
        if(countryResponse == null)
            return null;
        
        return countryResponse?.ToCountryResponse();
    }
}

//Check if countryAddRequest is not null
//Validate all properties of countryAddRequest
//Convert countryAddRequest from CountryAddRequest type to Country
//Generate a new CountryId
//Then add it into List<Country>
//Return CountryResponse object with generated CountryId