using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Vjezba.Model.Data;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

[Route("api/[controller]")]
public class SearchController : Controller
{
    private readonly IOwnerRepository _ownerRepo;
    private readonly IDogRepository _dogRepo;
    private readonly IWalkerRepository _walkerRepo;
    private readonly IBookingRepository _bookingRepo;
    private readonly IPaymentRepository _paymentRepo;
    private readonly IReviewRepository _reviewRepo;
    private readonly ApplicationDbContext _dbContext;

    public SearchController(IOwnerRepository ownerRepo, IDogRepository dogRepo, IWalkerRepository walkerRepo, IBookingRepository bookingRepo, IPaymentRepository paymentRepo, IReviewRepository reviewRepo, ApplicationDbContext dbContext)
    {
        _ownerRepo = ownerRepo;
        _dogRepo = dogRepo;
        _walkerRepo = walkerRepo;
        _bookingRepo = bookingRepo;
        _paymentRepo = paymentRepo;
        _reviewRepo = reviewRepo;
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

    private static string BookingStatusLabel(BookingStatus status) => status switch
    {
        BookingStatus.Pending => "Na čekanju",
        BookingStatus.Confirmed => "Potvrđeno",
        BookingStatus.Completed => "Završeno",
        BookingStatus.Cancelled => "Otkazano",
        _ => status.ToString()
    };

    [HttpGet("dogs")]
    public IActionResult Dogs(string q = "", int page = 1, int pageSize = 10)
    {
        const int MAX_PAGE_SIZE = 50;
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 1 : pageSize;
        pageSize = pageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : pageSize;

        var query = (q ?? string.Empty).Trim();
        if (query.Length > 200) query = query.Substring(0, 200);
        query = query.ToLower();

        var itemsQuery = ExcludeSoftDeleted(_dogRepo.Query())
            .Where(d => string.IsNullOrEmpty(query) || d.Name.ToLower().Contains(query) || d.Breed.ToLower().Contains(query) || d.Owner.Name.ToLower().Contains(query) || d.Owner.Surname.ToLower().Contains(query));

        var total = itemsQuery.Count();

        var items = itemsQuery
            .OrderBy(d => d.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new
            {
                id = d.Id,
                name = d.Name,
                breed = d.Breed,
                owner = d.Owner.Name + " " + d.Owner.Surname,
                age = d.Age,
                isVaccinated = d.IsVaccinated
            })
            .ToList();

        return Json(new { total, items });
    }

    [HttpGet("owners")]
    public IActionResult Owners(string q = "", int page = 1, int pageSize = 10)
    {
        const int MAX_PAGE_SIZE = 50;
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 1 : pageSize;
        pageSize = pageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : pageSize;

        var query = (q ?? string.Empty).Trim();
        if (query.Length > 200) query = query.Substring(0, 200);
        query = query.ToLower();

        var all = ExcludeSoftDeleted(_ownerRepo.GetAll().AsQueryable());
        var itemsQuery = all.Where(o => string.IsNullOrEmpty(query) || (o.Name + " " + o.Surname).ToLower().Contains(query) || (o.Email ?? string.Empty).ToLower().Contains(query));

        var total = itemsQuery.Count();
        var items = itemsQuery.OrderBy(o => o.Name).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(o => new
            {
                id = o.Id,
                name = o.Name + " " + o.Surname,
                address = o.Address,
                dogs = o.Dogs.Select(d => d.Name).ToList()
            })
            .ToList();

        return Json(new { total, items });
    }

    [HttpGet("walkers")]
    public IActionResult Walkers(string q = "", int page = 1, int pageSize = 10)
    {
        const int MAX_PAGE_SIZE = 50;
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 1 : pageSize;
        pageSize = pageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : pageSize;

        var query = (q ?? string.Empty).Trim();
        if (query.Length > 200) query = query.Substring(0, 200);
        query = query.ToLower();

        var all = ExcludeSoftDeleted(_walkerRepo.GetAll().AsQueryable());
        var itemsQuery = all.Where(w => string.IsNullOrEmpty(query) || (w.Name + " " + w.Surname).ToLower().Contains(query));

        var total = itemsQuery.Count();
        var items = itemsQuery.OrderBy(w => w.Name).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(w => new
            {
                id = w.Id,
                name = w.Name + " " + w.Surname,
                address = w.Address,
                rating = w.AverageRating,
                hourlyRate = w.HourlyRate,
                bookingCount = w.Bookings.Count
            })
            .ToList();

        return Json(new { total, items });
    }

    [HttpGet("bookings")]
    public IActionResult Bookings(string q = "", int page = 1, int pageSize = 10)
    {
        const int MAX_PAGE_SIZE = 50;
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 1 : pageSize;
        pageSize = pageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : pageSize;

        var query = (q ?? string.Empty).Trim();
        if (query.Length > 200) query = query.Substring(0, 200);
        query = query.ToLower();

        var itemsQuery = ExcludeSoftDeleted(_bookingRepo.Query())
            .Where(b => string.IsNullOrEmpty(query)
                || b.Owner.Name.ToLower().Contains(query)
                || b.Owner.Surname.ToLower().Contains(query)
                || b.DogWalker.Name.ToLower().Contains(query)
                || b.DogWalker.Surname.ToLower().Contains(query)
                || b.Dogs.Any(d => d.Name.ToLower().Contains(query)));

        var total = itemsQuery.Count();
        var items = itemsQuery.OrderBy(b => b.StartTime).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(b => new
            {
                id = b.Id,
                startTime = b.StartTime.ToString("dd.MM.yyyy HH:mm"),
                endTime = b.EndTime.ToString("HH:mm"),
                status = b.Status,
                owner = b.Owner.Name + " " + b.Owner.Surname,
                walker = b.DogWalker.Name + " " + b.DogWalker.Surname,
                dogs = b.Dogs.Select(d => d.Name).ToList()
            })
            .AsEnumerable()
            .Select(b => new
            {
                b.id,
                b.startTime,
                b.endTime,
                b.status,
                statusLabel = BookingStatusLabel(b.status),
                b.owner,
                b.walker,
                b.dogs
            })
            .ToList();

        return Json(new { total, items });
    }

    [HttpGet("payments")]
    public IActionResult Payments(string q = "", int page = 1, int pageSize = 10)
    {
        const int MAX_PAGE_SIZE = 50;
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 1 : pageSize;
        pageSize = pageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : pageSize;

        var query = (q ?? string.Empty).Trim();
        if (query.Length > 200) query = query.Substring(0, 200);
        query = query.ToLower();

        var itemsQuery = ExcludeSoftDeleted(_paymentRepo.Query())
            .Where(p => string.IsNullOrEmpty(query) || p.Booking.Owner.Name.ToLower().Contains(query) || p.Booking.DogWalker.Name.ToLower().Contains(query) || (p.PaymentMethod ?? string.Empty).ToLower().Contains(query));

        var total = itemsQuery.Count();
        var items = itemsQuery.OrderByDescending(p => p.Date).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(p => new
            {
                id = p.Id,
                amount = p.Amount,
                date = p.Date.ToString("dd.MM.yyyy HH:mm"),
                bookingId = p.Booking.Id,
                paymentMethod = p.PaymentMethod,
                isSuccessful = p.IsSuccessful
            })
            .ToList();

        return Json(new { total, items });
    }

    [HttpGet("reviews")]
    public IActionResult Reviews(string q = "", int page = 1, int pageSize = 10)
    {
        const int MAX_PAGE_SIZE = 50;
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 1 : pageSize;
        pageSize = pageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : pageSize;

        var query = (q ?? string.Empty).Trim();
        if (query.Length > 200) query = query.Substring(0, 200);
        query = query.ToLower();

        var itemsQuery = ExcludeSoftDeleted(_reviewRepo.Query())
            .Where(r => string.IsNullOrEmpty(query) || (r.Owner.Name + " " + r.Owner.Surname).ToLower().Contains(query) || (r.DogWalker.Name + " " + r.DogWalker.Surname).ToLower().Contains(query) || (r.Comment ?? string.Empty).ToLower().Contains(query));

        var total = itemsQuery.Count();
        var items = itemsQuery.OrderByDescending(r => r.Date).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(r => new { id = r.Id, rating = r.Rating, date = r.Date.ToString("dd.MM.yyyy HH:mm"), owner = r.Owner.Name + " " + r.Owner.Surname, walker = r.DogWalker.Name + " " + r.DogWalker.Surname, comment = r.Comment })
            .ToList();

        return Json(new { total, items });
    }
}
