using Microsoft.AspNetCore.Mvc;
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
        var walker = _walkerRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return walker is null ? NotFound() : View(walker);
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