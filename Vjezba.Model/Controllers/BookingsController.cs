using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

[Route("[controller]")]
public class BookingsController : Controller
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IWalkerRepository _walkerRepository;

    public BookingsController(IBookingRepository bookingRepository, IWalkerRepository walkerRepository)
    {
        _bookingRepository = bookingRepository;
        _walkerRepository = walkerRepository;
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
}