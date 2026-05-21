using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Repositories;

namespace Vjezba.Model.Controllers;

[Route("[controller]")]
public class ReviewsController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IWalkerRepository _walkerRepository;

    public ReviewsController(IReviewRepository reviewRepository, IOwnerRepository ownerRepository, IWalkerRepository walkerRepository)
    {
        _reviewRepository = reviewRepository;
        _ownerRepository = ownerRepository;
        _walkerRepository = walkerRepository;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index()
    {
        var items = _reviewRepository.GetAll().OrderByDescending(r => r.Date).ToList();
        return View(items);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var r = _reviewRepository.GetById(id);
        return r is null ? NotFound() : View(r);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new Review());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult CreatePost()
    {
        var form = Request.Form;

        if (!int.TryParse(form["OwnerId"], out var ownerId) || ownerId <= 0)
            ModelState.AddModelError("OwnerId", "Vlasnik je obavezan.");

        if (!int.TryParse(form["DogWalkerId"], out var walkerId) || walkerId <= 0)
            ModelState.AddModelError("DogWalkerId", "Šetač je obavezan.");

        if (!decimal.TryParse(form["Rating"], NumberStyles.Number, CultureInfo.InvariantCulture, out var rating)
            && !decimal.TryParse(form["Rating"], NumberStyles.Number, CultureInfo.CurrentCulture, out rating))
            ModelState.AddModelError("Rating", "Ocjena mora biti broj.");
        else if (rating < 0m || rating > 5m)
            ModelState.AddModelError("Rating", "Ocjena mora biti između 0 i 5.");

        var comment = form["Comment"].ToString().Trim();

        if (!DateTime.TryParseExact(form["Date"], "d.M.yyyy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            && !DateTime.TryParse(form["Date"], CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
            ModelState.AddModelError("Date", "Neispravan datum.");

        if (ownerId > 0 && !_ownerRepository.GetAll().Any(o => o.Id == ownerId))
            ModelState.AddModelError("OwnerId", "Odabrani vlasnik ne postoji.");

        if (walkerId > 0 && !_walkerRepository.GetAll().Any(w => w.Id == walkerId))
            ModelState.AddModelError("DogWalkerId", "Odabrani šetač ne postoji.");

        var model = new Review
        {
            OwnerId = ownerId,
            DogWalkerId = walkerId,
            Rating = rating,
            Comment = comment,
            Date = date
        };

        if (!ModelState.IsValid)
            return View("Create", model);

        _reviewRepository.Add(model);
        this.SetSuccessToast("Recenzija je uspješno dodana.");
        return RedirectToAction("Index");
    }

    [HttpGet("edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var r = _reviewRepository.GetById(id);
        if (r is null) return NotFound();
        return View(r);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(int id)
    {
        var r = _reviewRepository.GetById(id);
        if (r is null) return NotFound();

        var form = Request.Form;

        if (!int.TryParse(form["OwnerId"], out var ownerId) || ownerId <= 0)
            ModelState.AddModelError("OwnerId", "Vlasnik je obavezan.");

        if (!int.TryParse(form["DogWalkerId"], out var walkerId) || walkerId <= 0)
            ModelState.AddModelError("DogWalkerId", "Šetač je obavezan.");

        if (!decimal.TryParse(form["Rating"], NumberStyles.Number, CultureInfo.InvariantCulture, out var rating)
            && !decimal.TryParse(form["Rating"], NumberStyles.Number, CultureInfo.CurrentCulture, out rating))
            ModelState.AddModelError("Rating", "Ocjena mora biti broj.");
        else if (rating < 0m || rating > 5m)
            ModelState.AddModelError("Rating", "Ocjena mora biti između 0 i 5.");

        var comment = form["Comment"].ToString().Trim();

        if (!DateTime.TryParseExact(form["Date"], "d.M.yyyy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            && !DateTime.TryParse(form["Date"], CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
            ModelState.AddModelError("Date", "Neispravan datum.");

        if (ownerId > 0 && !_ownerRepository.GetAll().Any(o => o.Id == ownerId))
            ModelState.AddModelError("OwnerId", "Odabrani vlasnik ne postoji.");

        if (walkerId > 0 && !_walkerRepository.GetAll().Any(w => w.Id == walkerId))
            ModelState.AddModelError("DogWalkerId", "Odabrani šetač ne postoji.");

        r.OwnerId = ownerId;
        r.DogWalkerId = walkerId;
        r.Rating = rating;
        r.Comment = comment;
        r.Date = date;

        var updated = await TryUpdateModelAsync(r, "", x => x.OwnerId, x => x.DogWalkerId, x => x.Rating, x => x.Comment, x => x.Date);
        if (updated && ModelState.IsValid)
        {
            _reviewRepository.Update(r);
            this.SetSuccessToast("Recenzija je uspješno ažurirana.");
            return RedirectToAction("Index");
        }

        return View(r);
    }

    [HttpGet("delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        var r = _reviewRepository.GetById(id);
        if (r is null) return NotFound();
        return View(r);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var r = _reviewRepository.GetById(id);
        if (r is null) return NotFound();

        _reviewRepository.Delete(r);
        this.SetSuccessToast("Recenzija je uspješno obrisana.");
        return RedirectToAction("Index");
    }
}