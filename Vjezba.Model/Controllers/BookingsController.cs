using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Vjezba.Model;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

[Route("[controller]")]
public class BookingsController : Controller
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IWalkerRepository _walkerRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IDogRepository _dogRepository;

    public BookingsController(IBookingRepository bookingRepository, IWalkerRepository walkerRepository, IOwnerRepository ownerRepository, IDogRepository dogRepository)
    {
        _bookingRepository = bookingRepository;
        _walkerRepository = walkerRepository;
        _ownerRepository = ownerRepository;
        _dogRepository = dogRepository;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index()
    {
        var bookings = _bookingRepository.GetAll().OrderBy(booking => booking.StartTime).ToList();
        return View(bookings);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var booking = _bookingRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return booking is null ? NotFound() : View(booking);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        PopulateAutocompleteEndpoints();

        return View(new Booking());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult CreatePost()
    {
        var form = Request.Form;
        var booking = new Booking();

        if (!int.TryParse(form["OwnerId"], out var ownerId) || ownerId <= 0)
            ModelState.AddModelError("OwnerId", "Vlasnik je obavezan.");

        if (!int.TryParse(form["DogWalkerId"], out var walkerId) || walkerId <= 0)
            ModelState.AddModelError("DogWalkerId", "Šetač je obavezan.");

        var dogIds = form["DogIds"].Select(s => { int.TryParse(s, out var v); return v; }).Where(i => i > 0).ToList();
        if (!dogIds.Any())
            ModelState.AddModelError("DogIds", "Potrebno je odabrati barem jednog psa.");

        var startParsed = TryParseDateTime(form["StartTimeString"].ToString(), out var start);
        var endParsed = TryParseDateTime(form["EndTimeString"].ToString(), out var end);
        if (!startParsed)
            ModelState.AddModelError("StartTime", "Neispravan datum početka.");
        if (!endParsed)
            ModelState.AddModelError("EndTime", "Neispravan datum kraja.");

        if (startParsed && endParsed && end <= start)
            ModelState.AddModelError("EndTime", "Kraj rezervacije mora biti nakon početka.");

        if (!Enum.TryParse<BookingStatus>(form["Status"], true, out var status))
            ModelState.AddModelError("Status", "Status je obavezan.");

        if (ownerId > 0 && _ownerRepository.GetById(ownerId) is null)
            ModelState.AddModelError("OwnerId", "Odabrani vlasnik ne postoji.");

        if (walkerId > 0 && _walkerRepository.GetById(walkerId) is null)
            ModelState.AddModelError("DogWalkerId", "Odabrani šetač ne postoji.");

        var dogs = _dogRepository.GetTrackedByIds(dogIds);
        if (dogIds.Count != dogs.Count)
            ModelState.AddModelError("DogIds", "Jedan ili više odabranih pasa ne postoji.");

        booking.OwnerId = ownerId;
        booking.DogWalkerId = walkerId;
        booking.StartTime = start;
        booking.EndTime = end;
        booking.Status = status;
        foreach (var dog in dogs)
            booking.Dogs.Add(dog);

        if (!ModelState.IsValid)
        {
            PopulateAutocompleteEndpoints();
            return View("Create", booking);
        }

        // check overlap for walker
        var overlapping = _bookingRepository.Query()
            .Where(b => b.DogWalkerId == walkerId && !(b.EndTime <= start || b.StartTime >= end))
            .Any();
        if (overlapping)
        {
            ModelState.AddModelError(string.Empty, "Odabrani šetač ima drugu rezervaciju u tom terminu.");
            PopulateAutocompleteEndpoints();
            return View("Create", booking);
        }

        _bookingRepository.Add(booking);
        this.SetSuccessToast("Rezervacija je uspješno dodana.");
        return RedirectToAction("Index");
    }

    [HttpGet("edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var booking = _bookingRepository.GetById(id);
        if (booking is null) return NotFound();

        PopulateAutocompleteEndpoints();

        return View(booking);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Edit")]
    public IActionResult EditPost(int id)
    {
        var booking = _bookingRepository.GetById(id);
        if (booking is null) return NotFound();

        var form = Request.Form;
        if (!int.TryParse(form["OwnerId"], out var ownerId) || ownerId <= 0)
            ModelState.AddModelError("OwnerId", "Vlasnik je obavezan.");

        if (!int.TryParse(form["DogWalkerId"], out var walkerId) || walkerId <= 0)
            ModelState.AddModelError("DogWalkerId", "Šetač je obavezan.");

        var dogIds = form["DogIds"].Select(s => { int.TryParse(s, out var v); return v; }).Where(i => i > 0).ToList();
        if (!dogIds.Any())
            ModelState.AddModelError("DogIds", "Potrebno je odabrati barem jednog psa.");

        var startParsed = TryParseDateTime(form["StartTimeString"].ToString(), out var start);
        var endParsed = TryParseDateTime(form["EndTimeString"].ToString(), out var end);
        if (!startParsed)
            ModelState.AddModelError("StartTime", "Neispravan datum početka.");
        if (!endParsed)
            ModelState.AddModelError("EndTime", "Neispravan datum kraja.");

        if (startParsed && endParsed && end <= start)
            ModelState.AddModelError("EndTime", "Kraj rezervacije mora biti nakon početka.");

        if (!Enum.TryParse<BookingStatus>(form["Status"], true, out var status))
            ModelState.AddModelError("Status", "Status je obavezan.");

        if (ownerId > 0 && _ownerRepository.GetById(ownerId) is null)
            ModelState.AddModelError("OwnerId", "Odabrani vlasnik ne postoji.");

        if (walkerId > 0 && _walkerRepository.GetById(walkerId) is null)
            ModelState.AddModelError("DogWalkerId", "Odabrani šetač ne postoji.");

        var dogs = _dogRepository.GetTrackedByIds(dogIds);
        if (dogIds.Count != dogs.Count)
            ModelState.AddModelError("DogIds", "Jedan ili više odabranih pasa ne postoji.");

        if (!ModelState.IsValid)
        {
            booking.OwnerId = ownerId;
            booking.DogWalkerId = walkerId;
            booking.StartTime = start;
            booking.EndTime = end;
            booking.Status = status;
            booking.Dogs.Clear();
            foreach (var dog in dogs)
                booking.Dogs.Add(dog);

            PopulateAutocompleteEndpoints();
            return View(booking);
        }

        var overlapping = _bookingRepository.Query()
            .Where(b => b.Id != id && b.DogWalkerId == walkerId && !(b.EndTime <= start || b.StartTime >= end))
            .Any();
        if (overlapping)
        {
            ModelState.AddModelError(string.Empty, "Odabrani šetač ima drugu rezervaciju u tom terminu.");
            PopulateAutocompleteEndpoints();
            return View(booking);
        }

        booking.OwnerId = ownerId;
        booking.DogWalkerId = walkerId;
        booking.StartTime = start;
        booking.EndTime = end;
        booking.Status = status;

        // update dogs
        booking.Dogs.Clear();
        foreach (var d in dogs) booking.Dogs.Add(d);

        _bookingRepository.Update(booking);
        this.SetSuccessToast("Rezervacija je uspješno ažurirana.");
        return RedirectToAction("Index");
    }

    [HttpGet("delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        var booking = _bookingRepository.GetById(id);
        if (booking is null) return NotFound();

        return View(booking);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var booking = _bookingRepository.GetById(id);
        if (booking is null) return NotFound();

        if (booking.Payments.Any())
        {
            ModelState.AddModelError(string.Empty, "Ne možete obrisati rezervaciju koja ima uplate.");
            return View(booking);
        }

        _bookingRepository.Delete(booking);
        this.SetSuccessToast("Rezervacija je uspješno obrisana.");
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Custom route: /šetač/{walkerSlug}/rezervacije
    /// Lists all reservations for a specific walker
    /// </summary>
    [HttpGet("~/šetač/{walkerSlug}/rezervacije")]
    public IActionResult ByWalkerSlug(string walkerSlug)
    {
        var walker = _walkerRepository.GetAll()
            .FirstOrDefault(item => CreateSlug($"{item.Name} {item.Surname}") == walkerSlug);

        if (walker is null)
            return NotFound($"No walker found for slug {walkerSlug}");

        var bookings = _bookingRepository.GetAll()
            .Where(b => b.DogWalker.Id == walker.Id)
            .OrderBy(b => b.StartTime)
            .ToList();
        
        if (!bookings.Any())
            return NotFound($"No bookings found for walker {walker.Name} {walker.Surname}");
        
        return View("Index", bookings);
    }

    [HttpGet("~/šetač/{walkerId:int}/rezervacije")]
    public IActionResult ByWalker(int walkerId)
    {
        var walker = _walkerRepository.GetAll().FirstOrDefault(item => item.Id == walkerId);

        if (walker is null)
            return NotFound($"No walker found with id {walkerId}");

        return RedirectToActionPermanent(nameof(ByWalkerSlug), new
        {
            walkerSlug = CreateSlug($"{walker.Name} {walker.Surname}")
        });
    }

    private static string CreateSlug(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var character in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(character);
            if (category == UnicodeCategory.NonSpacingMark)
                continue;

            if (char.IsLetterOrDigit(character))
            {
                builder.Append(char.ToLowerInvariant(character));
                continue;
            }

            if (char.IsWhiteSpace(character) || character == '-' || character == '_')
                builder.Append('-');
        }

        return builder.ToString().Trim('-');
    }

    private void PopulateAutocompleteEndpoints()
    {
        ViewBag.OwnerEndpoint = "/api/autocomplete/owners";
        ViewBag.WalkerEndpoint = "/api/autocomplete/walkers";
        ViewBag.DogEndpoint = "/api/autocomplete/dogs";
    }

    private static bool TryParseDateTime(string? value, out DateTime parsed)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            parsed = default;
            return false;
        }

        return DateTime.TryParseExact(value, "d.M.yyyy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed)
            || DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed);
    }
}