using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

[Route("[controller]")]
public class PaymentsController : Controller
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentsController(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index()
    {
        var payments = _paymentRepository.GetAll().OrderByDescending(payment => payment.Date).ToList();
        return View(payments);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var payment = _paymentRepository.GetAll().FirstOrDefault(item => item.Id == id);
        return payment is null ? NotFound() : View(payment);
    }

    /// <summary>
    /// Custom route: /uplate/{year}/{month}
    /// List payments filtered by year and month (Croatian: uplate = payments)
    /// Example: /uplate/2026/05 for May 2026
    /// </summary>
    [HttpGet("~/uplate/{year:int}/{month:int}")]
    public IActionResult ByPeriod(int year, int month)
    {
        if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12");
        
        var payments = _paymentRepository.GetAll()
            .Where(p => p.Date.Year == year && p.Date.Month == month)
            .OrderByDescending(p => p.Date)
            .ToList();
        
        if (!payments.Any())
            return NotFound($"No payments found for {month:D2}/{year}");
        
        return View("Index", payments);
    }
}