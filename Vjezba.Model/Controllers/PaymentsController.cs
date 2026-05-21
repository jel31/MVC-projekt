using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Repositories;
using System.Globalization;

namespace Vjezba.Model.Controllers;

[Route("[controller]")]
public class PaymentsController : Controller
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingRepository _bookingRepository;

    public PaymentsController(IPaymentRepository paymentRepository, IBookingRepository bookingRepository)
    {
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index()
    {
        var items = _paymentRepository.GetAll().OrderByDescending(p => p.Date).ToList();
        return View(items);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var p = _paymentRepository.GetById(id);
        return p is null ? NotFound() : View(p);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new Payment());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult CreatePost()
    {
        var form = Request.Form;

        if (!int.TryParse(form["BookingId"], out var bookingId) || bookingId <= 0)
            ModelState.AddModelError("BookingId", "Rezervacija je obavezna.");

        if (!decimal.TryParse(form["Amount"], NumberStyles.Number, CultureInfo.InvariantCulture, out var amount)
            && !decimal.TryParse(form["Amount"], NumberStyles.Number, CultureInfo.CurrentCulture, out amount))
            ModelState.AddModelError("Amount", "Iznos mora biti broj.");

        if (!DateTime.TryParseExact(form["Date"], "d.M.yyyy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            && !DateTime.TryParse(form["Date"], CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
            ModelState.AddModelError("Date", "Neispravan datum.");

        var paymentMethod = form["PaymentMethod"].ToString().Trim();
        if (string.IsNullOrWhiteSpace(paymentMethod))
            ModelState.AddModelError("PaymentMethod", "Metoda plaćanja je obavezna.");

        var isSuccessfulRaw = form["IsSuccessful"].ToString();
        var isSuccessful = string.Equals(isSuccessfulRaw, "true", StringComparison.OrdinalIgnoreCase)
            || string.Equals(isSuccessfulRaw, "on", StringComparison.OrdinalIgnoreCase);

        if (bookingId > 0 && _bookingRepository.GetById(bookingId) is null)
            ModelState.AddModelError("BookingId", "Odabrana rezervacija ne postoji.");

        var model = new Payment
        {
            BookingId = bookingId,
            Amount = amount,
            Date = date,
            PaymentMethod = paymentMethod,
            IsSuccessful = isSuccessful
        };

        if (!ModelState.IsValid)
            return View("Create", model);

        _paymentRepository.Add(model);
        this.SetSuccessToast("Uplata je uspješno dodana.");
        return RedirectToAction("Index");
    }

    [HttpGet("edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var p = _paymentRepository.GetById(id);
        if (p is null) return NotFound();
        return View(p);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(int id)
    {
        var p = _paymentRepository.GetById(id);
        if (p is null) return NotFound();

        var form = Request.Form;

        if (!int.TryParse(form["BookingId"], out var bookingId) || bookingId <= 0)
            ModelState.AddModelError("BookingId", "Rezervacija je obavezna.");

        if (!decimal.TryParse(form["Amount"], NumberStyles.Number, CultureInfo.InvariantCulture, out var amount)
            && !decimal.TryParse(form["Amount"], NumberStyles.Number, CultureInfo.CurrentCulture, out amount))
            ModelState.AddModelError("Amount", "Iznos mora biti broj.");
        else if (amount <= 0)
            ModelState.AddModelError("Amount", "Iznos mora biti veći od 0.");

        if (!DateTime.TryParseExact(form["Date"], "d.M.yyyy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            && !DateTime.TryParse(form["Date"], CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
            ModelState.AddModelError("Date", "Neispravan datum.");

        var paymentMethod = form["PaymentMethod"].ToString().Trim();
        if (string.IsNullOrWhiteSpace(paymentMethod))
            ModelState.AddModelError("PaymentMethod", "Metoda plaćanja je obavezna.");

        var isSuccessfulRaw = form["IsSuccessful"].ToString();
        var isSuccessful = string.Equals(isSuccessfulRaw, "true", StringComparison.OrdinalIgnoreCase)
            || string.Equals(isSuccessfulRaw, "on", StringComparison.OrdinalIgnoreCase);

        if (bookingId > 0 && _bookingRepository.GetById(bookingId) is null)
            ModelState.AddModelError("BookingId", "Odabrana rezervacija ne postoji.");

        p.BookingId = bookingId;
        p.Amount = amount;
        p.Date = date;
        p.PaymentMethod = paymentMethod;
        p.IsSuccessful = isSuccessful;

        var updated = await TryUpdateModelAsync(p, "", x => x.BookingId, x => x.Amount, x => x.Date, x => x.PaymentMethod, x => x.IsSuccessful);
        if (updated && ModelState.IsValid)
        {
            _paymentRepository.Update(p);
            this.SetSuccessToast("Uplata je uspješno ažurirana.");
            return RedirectToAction("Index");
        }

        return View(p);
    }

    [HttpGet("delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        var p = _paymentRepository.GetById(id);
        if (p is null) return NotFound();
        return View(p);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var p = _paymentRepository.GetById(id);
        if (p is null) return NotFound();

        _paymentRepository.Delete(p);
        this.SetSuccessToast("Uplata je uspješno obrisana.");
        return RedirectToAction("Index");
    }
}
