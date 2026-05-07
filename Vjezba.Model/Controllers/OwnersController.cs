using Microsoft.AspNetCore.Mvc;
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
        var owner = _ownerRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return owner is null ? NotFound() : View(owner);
    }
}