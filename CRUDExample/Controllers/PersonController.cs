using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers;

[Route("person")]
public class PersonController : Controller
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;

    public PersonController(IPersonsService personsService, ICountriesService countriesService)
    {
        _personsService = personsService;
        _countriesService = countriesService;
    }
    
    [Route("index")]
    [Route("/")]
    public IActionResult Index(string searchBy, string? searchText, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.Asc)
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(PersonResponse.PersonName), "Person Name" },
            { nameof(PersonResponse.Email), "Email" },
            { nameof(PersonResponse.DateOfBirth), "Date Of Birth" },
            { nameof(PersonResponse.Gender), "Gender" },
            { nameof(PersonResponse.CountryId), "Country" },
            { nameof(PersonResponse.Address), "Address" },
        };
        
        List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchText);

        List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder;
        return View(persons);
    }
    
    //Executes when the user clicks on "Create Person" hyperlink
    [Route("create")]
    [HttpGet]
    public IActionResult Create()
    {
        List<CountryResponse> countries =  _countriesService.GetAllCountries();
        ViewBag.Countries = countries;
        
        return View();
    }

    [Route("create")]
    [HttpPost]
    public IActionResult Create(PersonAddRequest personAddRequest)
    {
        if (!ModelState.IsValid)
        {   
            List<CountryResponse> countries =  _countriesService.GetAllCountries();
            ViewBag.Countries = countries;

            ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).SelectMany(e => e.ErrorMessage).ToList();
            
            return View();
        }
        
        PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
        return RedirectToAction("Index", "Person");
    }
}