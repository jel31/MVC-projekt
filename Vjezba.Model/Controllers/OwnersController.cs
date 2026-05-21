using Microsoft.AspNetCore.Mvc;
using Vjezba.Model;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

public class OwnersController : Controller
{
    private readonly IOwnerRepository _ownerRepository;

    public OwnersController(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    public IActionResult Index()
    {
        var owners = _ownerRepository.GetAll().OrderBy(owner => owner.Surname).ThenBy(owner => owner.Name).ToList();
        return View(owners);
    }

    public IActionResult Details(int id)
    {
        var owner = _ownerRepository.GetById(id);
        return owner is null ? NotFound() : View(owner);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new DogOwner());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(DogOwner owner)
    {
        if (!ModelState.IsValid)
        {
            return View(owner);
        }

        _ownerRepository.Add(owner);
        this.SetSuccessToast("Vlasnik je uspješno dodan.");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var owner = _ownerRepository.GetById(id);
        return owner is null ? NotFound() : View(owner);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var owner = _ownerRepository.GetById(id);
        if (owner is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(
            owner,
            string.Empty,
            currentOwner => currentOwner.Name,
            currentOwner => currentOwner.Surname,
            currentOwner => currentOwner.Email,
            currentOwner => currentOwner.PhoneNumber,
            currentOwner => currentOwner.Address);

        if (!updated || !ModelState.IsValid)
        {
            return View(owner);
        }

        _ownerRepository.Update(owner);
        this.SetSuccessToast("Podaci vlasnika su uspješno spremljeni.");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var owner = _ownerRepository.GetById(id);
        return owner is null ? NotFound() : View(owner);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var owner = _ownerRepository.GetById(id);
        if (owner is null)
        {
            return NotFound();
        }

        if (owner.Dogs.Any() || owner.Bookings.Any())
        {
            ModelState.AddModelError(string.Empty, "Vlasnika nije moguće obrisati dok ima povezane pse ili rezervacije.");
            return View("Delete", owner);
        }

        _ownerRepository.Delete(owner);
        this.SetSuccessToast("Vlasnik je uspješno obrisan.");
        return RedirectToAction(nameof(Index));
    }
}