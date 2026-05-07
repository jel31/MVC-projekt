using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

public class ReviewsController : Controller
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewsController(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public IActionResult Index()
    {
        var reviews = _reviewRepository.GetAll().OrderByDescending(review => review.Date).ToList();
        return View(reviews);
    }

    public IActionResult Details(int id)
    {
        var review = _reviewRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return review is null ? NotFound() : View(review);
    }
}