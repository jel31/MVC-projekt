using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Models;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

[Route("[controller]")]
public class DogsController : Controller
{
    private readonly IDogRepository _dogRepository;

    public DogsController(IDogRepository dogRepository)
    {
        _dogRepository = dogRepository;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index([FromQuery] DogFilterViewModel filter)
    {
        var query = _dogRepository.Query();

        if (!string.IsNullOrWhiteSpace(filter.Breed))
        {
            query = query.Where(dog => dog.Breed.Contains(filter.Breed));
        }

        var items = query
            .OrderBy(dog => dog.Owner.Surname)
            .ThenBy(dog => dog.Name)
            .ToList();

        var viewModel = new DogIndexPageViewModel
        {
            Items = items,
            Filter = filter
        };

        return View(viewModel);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var dog = _dogRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return dog is null ? NotFound() : View(dog);
    }

    /// <summary>
    /// Custom route: /psi/{ownerSurname}/{dogName}
    /// Search dogs by owner surname and dog name (Croatian-friendly)
    /// Example: /psi/horvat/rex
    /// </summary>
    [HttpGet("~/psi/{ownerSurname:alpha}/{dogName:alpha}")]
    public IActionResult ByOwnerAndName(string ownerSurname, string dogName)
    {
        var dog = _dogRepository.GetAll()
            .FirstOrDefault(d => 
                d.Owner.Surname.ToLower() == ownerSurname.ToLower() &&
                d.Name.ToLower() == dogName.ToLower());
        
        return dog is null ? NotFound($"Dog '{dogName}' owned by '{ownerSurname}' not found") : View("Details", dog);
    }
}