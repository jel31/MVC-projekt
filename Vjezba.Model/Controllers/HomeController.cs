using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

public class HomeController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IWalkerRepository _walkerRepository;
    private readonly IDogRepository _dogRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IReviewRepository _reviewRepository;

    public HomeController(
        IOwnerRepository ownerRepository,
        IWalkerRepository walkerRepository,
        IDogRepository dogRepository,
        IBookingRepository bookingRepository,
        IPaymentRepository paymentRepository,
        IReviewRepository reviewRepository)
    {
        _ownerRepository = ownerRepository;
        _walkerRepository = walkerRepository;
        _dogRepository = dogRepository;
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _reviewRepository = reviewRepository;
    }

    public IActionResult Index()
    {
        var model = new HomeViewModel
        {
            Owners = _ownerRepository.GetAll(),
            Walkers = _walkerRepository.GetAll(),
            Dogs = _dogRepository.GetAll(),
            Bookings = _bookingRepository.GetAll(),
            Payments = _paymentRepository.GetAll(),
            Reviews = _reviewRepository.GetAll()
        };

        return View(model);
    }

    public IActionResult Owners()
    {
        return View(_ownerRepository.GetAll().OrderBy(owner => owner.Surname).ThenBy(owner => owner.Name).ToList());
    }

    public IActionResult Owner(int id)
    {
        var owner = _ownerRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return owner is null ? NotFound() : View(owner);
    }

    public IActionResult Walkers()
    {
        return View(_walkerRepository.GetAll().OrderByDescending(walker => walker.AverageRating).ThenBy(walker => walker.Surname).ThenBy(walker => walker.Name).ToList());
    }

    public IActionResult Walker(int id)
    {
        var walker = _walkerRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return walker is null ? NotFound() : View(walker);
    }

    public IActionResult Dogs()
    {
        return View(_dogRepository.GetAll().OrderBy(dog => dog.Owner.Surname).ThenBy(dog => dog.Name).ToList());
    }

    public IActionResult Dog(int id)
    {
        var dog = _dogRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return dog is null ? NotFound() : View(dog);
    }

    public IActionResult Bookings()
    {
        return View(_bookingRepository.GetAll().OrderBy(booking => booking.StartTime).ToList());
    }

    public IActionResult Booking(int id)
    {
        var booking = _bookingRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return booking is null ? NotFound() : View(booking);
    }

    public IActionResult Payments()
    {
        return View(_paymentRepository.GetAll().OrderByDescending(payment => payment.Date).ToList());
    }

    public IActionResult Payment(int id)
    {
        var payment = _paymentRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return payment is null ? NotFound() : View(payment);
    }

    public IActionResult Reviews()
    {
        return View(_reviewRepository.GetAll().OrderByDescending(review => review.Date).ToList());
    }

    public IActionResult Review(int id)
    {
        var review = _reviewRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return review is null ? NotFound() : View(review);
    }
}
