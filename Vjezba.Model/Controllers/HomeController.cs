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
}
