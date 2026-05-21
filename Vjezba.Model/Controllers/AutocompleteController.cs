using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Vjezba.Model.Data;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

[Route("api/[controller]")]
public class AutocompleteController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IDogRepository _dogRepository;
    private readonly IWalkerRepository _walkerRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ApplicationDbContext _dbContext;

    public AutocompleteController(IOwnerRepository ownerRepository, IDogRepository dogRepository, IWalkerRepository walkerRepository, IBookingRepository bookingRepository, ApplicationDbContext dbContext)
    {
        _ownerRepository = ownerRepository;
        _dogRepository = dogRepository;
        _walkerRepository = walkerRepository;
        _bookingRepository = bookingRepository;
        _dbContext = dbContext;
    }

    private IQueryable<T> ExcludeSoftDeleted<T>(IQueryable<T> query)
    {
        var et = _dbContext.Model.FindEntityType(typeof(T));
        if (et != null && et.FindProperty("IsDeleted") != null)
        {
            return query.Where(e => !EF.Property<bool>(e!, "IsDeleted"));
        }

        return query;
    }

    [HttpGet("owners")]
    public IActionResult Owners(string q, int? id)
    {
        if (id.HasValue)
        {
            var owner = ExcludeSoftDeleted(_ownerRepository.GetAll().AsQueryable())
                .Where(o => o.Id == id.Value)
                .Select(o => new { id = o.Id, text = o.Name + " " + o.Surname })
                .FirstOrDefault();

            return Json(owner is null ? [] : new[] { owner });
        }

        var query = (q ?? string.Empty).Trim();
        if (query.Length > 200) query = query.Substring(0, 200);
        query = query.ToLower();

        var items = ExcludeSoftDeleted(_ownerRepository.GetAll().AsQueryable())
            .Where(o => string.IsNullOrEmpty(query) || (o.Name + " " + o.Surname).ToLower().Contains(query) || (o.Email ?? string.Empty).ToLower().Contains(query))
            .Select(o => new { id = o.Id, text = o.Name + " " + o.Surname })
            .Take(10)
            .ToList();

        return Json(items);
    }

    [HttpGet("dogs")]
    public IActionResult Dogs(string q, int? id)
    {
        if (id.HasValue)
        {
            var dog = ExcludeSoftDeleted(_dogRepository.Query())
                .Where(d => d.Id == id.Value)
                .Select(d => new { id = d.Id, text = d.Name + " — " + d.Owner.Name + " " + d.Owner.Surname })
                .FirstOrDefault();

            return Json(dog is null ? [] : new[] { dog });
        }

        var query = (q ?? string.Empty).Trim();
        if (query.Length > 200) query = query.Substring(0, 200);
        query = query.ToLower();

        var items = ExcludeSoftDeleted(_dogRepository.Query())
            .Where(d => string.IsNullOrEmpty(query) || d.Name.ToLower().Contains(query) || d.Breed.ToLower().Contains(query))
            .Select(d => new { id = d.Id, text = d.Name + " — " + d.Owner.Name + " " + d.Owner.Surname })
            .Take(10)
            .ToList();

        return Json(items);
    }

    [HttpGet("walkers")]
    public IActionResult Walkers(string q, int? id)
    {
        if (id.HasValue)
        {
            var walker = ExcludeSoftDeleted(_walkerRepository.GetAll().AsQueryable())
                .Where(w => w.Id == id.Value)
                .Select(w => new { id = w.Id, text = w.Name + " " + w.Surname })
                .FirstOrDefault();

            return Json(walker is null ? [] : new[] { walker });
        }

        var query = (q ?? string.Empty).Trim();
        if (query.Length > 200) query = query.Substring(0, 200);
        query = query.ToLower();

        var items = ExcludeSoftDeleted(_walkerRepository.GetAll().AsQueryable())
            .Where(w => string.IsNullOrEmpty(query) || (w.Name + " " + w.Surname).ToLower().Contains(query))
            .Select(w => new { id = w.Id, text = w.Name + " " + w.Surname })
            .Take(10)
            .ToList();

        return Json(items);
    }

    [HttpGet("bookings")]
    public IActionResult Bookings(string q, int? id)
    {
        if (id.HasValue)
        {
            var booking = ExcludeSoftDeleted(_bookingRepository.Query())
                .Where(b => b.Id == id.Value)
                .Select(b => new { id = b.Id, text = $"#{b.Id} {b.StartTime:dd.MM.yyyy HH:mm} — {b.DogWalker.Name} {b.DogWalker.Surname}" })
                .FirstOrDefault();

            return Json(booking is null ? [] : new[] { booking });
        }

        var query = (q ?? string.Empty).Trim();
        if (query.Length > 200) query = query.Substring(0, 200);
        query = query.ToLower();

        var items = ExcludeSoftDeleted(_bookingRepository.Query())
            .Where(b => string.IsNullOrEmpty(query) || b.Owner.Name.ToLower().Contains(query) || b.DogWalker.Name.ToLower().Contains(query))
            .Select(b => new { id = b.Id, text = $"#{b.Id} {b.StartTime:dd.MM.yyyy HH:mm} — {b.DogWalker.Name} {b.DogWalker.Surname}" })
            .Take(10)
            .ToList();

        return Json(items);
    }
}
