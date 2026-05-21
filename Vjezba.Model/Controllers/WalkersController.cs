using Microsoft.AspNetCore.Mvc;
using Vjezba.Model;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

[Route("[controller]")]
public class WalkersController : Controller
{
    private readonly IWalkerRepository _walkerRepository;

    public WalkersController(IWalkerRepository walkerRepository)
    {
        _walkerRepository = walkerRepository;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index()
    {
        var walkers = _walkerRepository.GetAll().OrderByDescending(walker => walker.AverageRating).ThenBy(walker => walker.Surname).ThenBy(walker => walker.Name).ToList();
        return View(walkers);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var walker = _walkerRepository.GetById(id);
        return walker is null ? NotFound() : View(walker);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new DogWalker());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(DogWalker walker)
    {
        if (!ModelState.IsValid)
        {
            return View(walker);
        }

        _walkerRepository.Add(walker);
        this.SetSuccessToast("Šetač je uspješno dodan.");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var walker = _walkerRepository.GetById(id);
        return walker is null ? NotFound() : View(walker);
    }

    [HttpPost("edit/{id:int}")]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var walker = _walkerRepository.GetById(id);
        if (walker is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(
            walker,
            string.Empty,
            currentWalker => currentWalker.Name,
            currentWalker => currentWalker.Surname,
            currentWalker => currentWalker.Email,
            currentWalker => currentWalker.PhoneNumber,
            currentWalker => currentWalker.Address,
            currentWalker => currentWalker.HourlyRate);

        if (!updated || !ModelState.IsValid)
        {
            return View(walker);
        }

        _walkerRepository.Update(walker);
        this.SetSuccessToast("Podaci šetača su uspješno spremljeni.");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        var walker = _walkerRepository.GetById(id);
        return walker is null ? NotFound() : View(walker);
    }

    [HttpPost("delete/{id:int}")]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var walker = _walkerRepository.GetById(id);
        if (walker is null)
        {
            return NotFound();
        }

        if (walker.Bookings.Any() || walker.Reviews.Any())
        {
            ModelState.AddModelError(string.Empty, "Šetača nije moguće obrisati dok ima povezane rezervacije ili recenzije.");
            return View("Delete", walker);
        }

        _walkerRepository.Delete(walker);
        this.SetSuccessToast("Šetač je uspješno obrisan.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Custom route: /najbolji-šetači
    /// List top-rated walkers (Croatian: najbolji = best, šetači = walkers)
    /// Example: /najbolji-šetači shows best 10 walkers by rating
    /// </summary>
    [HttpGet("~/najbolji-šetači")]
    public IActionResult TopRated(int limit = 10)
    {
        var topWalkers = _walkerRepository.GetAll()
            .OrderByDescending(w => w.AverageRating)
            .Take(limit)
            .ToList();
        
        return View("Index", topWalkers);
    }
}