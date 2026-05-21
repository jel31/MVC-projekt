using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Models;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

[Route("[controller]")]
public class DogsController : Controller
{
    private readonly IDogRepository _dogRepository;
    private readonly IOwnerRepository _ownerRepository;

    public DogsController(IDogRepository dogRepository, IOwnerRepository ownerRepository)
    {
        _dogRepository = dogRepository;
        _ownerRepository = ownerRepository;
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

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.OwnerSelectList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            _ownerRepository.GetAll().Select(o => new { o.Id, FullName = o.Name + " " + o.Surname }),
            "Id", "FullName");

        return View(new Dog());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Dog dog)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.OwnerSelectList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _ownerRepository.GetAll().Select(o => new { o.Id, FullName = o.Name + " " + o.Surname }),
                "Id", "FullName", dog.OwnerId);

            return View(dog);
        }

        _dogRepository.Add(dog);
        this.SetSuccessToast("Pas je uspješno dodan.");
        return RedirectToAction("Index");
    }

    [HttpGet("edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var dog = _dogRepository.GetById(id);
        if (dog is null) return NotFound();

        ViewBag.OwnerSelectList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            _ownerRepository.GetAll().Select(o => new { o.Id, FullName = o.Name + " " + o.Surname }),
            "Id", "FullName", dog.OwnerId);

        return View(dog);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(int id)
    {
        var dog = _dogRepository.GetById(id);
        if (dog is null) return NotFound();

        var updated = await TryUpdateModelAsync(dog, "", d => d.Name, d => d.Breed, d => d.Age, d => d.IsVaccinated, d => d.IsFriendly, d => d.OwnerId);
        if (updated && ModelState.IsValid)
        {
            _dogRepository.Update(dog);
            this.SetSuccessToast("Podaci psa su uspješno spremljeni.");
            return RedirectToAction("Index");
        }

        ViewBag.OwnerSelectList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            _ownerRepository.GetAll().Select(o => new { o.Id, FullName = o.Name + " " + o.Surname }),
            "Id", "FullName", dog.OwnerId);

        return View(dog);
    }

    [HttpGet("delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        var dog = _dogRepository.GetById(id);
        if (dog is null) return NotFound();

        return View(dog);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var dog = _dogRepository.GetById(id);
        if (dog is null) return NotFound();

        if (dog.Bookings.Any())
        {
            ModelState.AddModelError(string.Empty, "Ne možete obrisati psa koji ima povezane rezervacije.");
            return View(dog);
        }

        _dogRepository.Delete(dog);
        this.SetSuccessToast("Pas je uspješno obrisan.");
        return RedirectToAction("Index");
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