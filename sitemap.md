# Sitemap for Šapice u pokretu

This document lists reachable URLs, the handling controller action, HTTP verb, and the Razor view rendered. Includes custom attribute routes.

- URL: `/` or `/Home/Index`
  - Controller.Action: `HomeController.Index`
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Home/Index.cshtml](Vjezba.Model/Views/Home/Index.cshtml)
  - Observed: HTTP 200 — Title: Početna - Šapice u pokretu

- URL: `/Owners` or `/Owners/Index`
  - Controller.Action: `OwnersController.Index`
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Owners/Index.cshtml](Vjezba.Model/Views/Owners/Index.cshtml)
  - Observed: not tested (200 expected)

- URL: `/Owners/Details/{id}`
  - Controller.Action: `OwnersController.Details` (id)
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Owners/Details.cshtml](Vjezba.Model/Views/Owners/Details.cshtml)
  - Observed: not tested (200/404 depend on id)

- URL: `/Dogs` or `/Dogs/Index`
  - Controller.Action: `DogsController.Index`
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Dogs/Index.cshtml](Vjezba.Model/Views/Dogs/Index.cshtml)
  - Observed: HTTP 200 — Title: Psi - Šapice u pokretu

- URL: `/Dogs/{id}`
  - Controller.Action: `DogsController.Details` (id)
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Dogs/Details.cshtml](Vjezba.Model/Views/Dogs/Details.cshtml)
  - Observed: HTTP 200 (for tested id=1) — Title: Detalji psa - Šapice u pokretu

- URL: `/psi/{ownerSurname}/{dogName}`  (custom attribute route)
  - Controller.Action: `DogsController.ByOwnerAndName` (ownerSurname, dogName)
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Dogs/Details.cshtml](Vjezba.Model/Views/Dogs/Details.cshtml) (returned using `View("Details", dog)`)
  - Example: `/psi/horvat/rex`
  - Observed: HTTP 200 for `/psi/horvat/rex` — Title: Detalji psa - Šapice u pokretu

- URL: `/Walkers` or `/Walkers/Index`
  - Controller.Action: `WalkersController.Index`
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Walkers/Index.cshtml](Vjezba.Model/Views/Walkers/Index.cshtml)
  - Observed: not tested (200 expected)

- URL: `/Walkers/{id}`
  - Controller.Action: `WalkersController.Details` (id)
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Walkers/Details.cshtml](Vjezba.Model/Views/Walkers/Details.cshtml)
  - Observed: not tested (200/404 depend on id)

- URL: `/najbolji-šetači`  (custom attribute route)
  - Controller.Action: `WalkersController.TopRated` (optional query `limit`)
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Walkers/Index.cshtml](Vjezba.Model/Views/Walkers/Index.cshtml) (returned using `View("Index", topWalkers)`)
  - Example: `/najbolji-šetači?limit=5`
  - Observed: HTTP 200 — Title: Šetači - Šapice u pokretu

- URL: `/Bookings` or `/Bookings/Index`
  - Controller.Action: `BookingsController.Index`
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Bookings/Index.cshtml](Vjezba.Model/Views/Bookings/Index.cshtml)
  - Observed: not tested (200 expected)

- URL: `/Bookings/{id}`
  - Controller.Action: `BookingsController.Details` (id)
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Bookings/Details.cshtml](Vjezba.Model/Views/Bookings/Details.cshtml)
  - Observed: not tested (200/404 depend on id)

- URL: `/šetač/{walkerSlug}/rezervacije`  (custom attribute route)
  - Controller.Action: `BookingsController.ByWalkerSlug` (walkerSlug)
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Bookings/Index.cshtml](Vjezba.Model/Views/Bookings/Index.cshtml) (returned using `View("Index", bookings)`)
  - Example slug: `/šetač/ivan-horvat/rezervacije`
  - Observed: route exists; tested `/šetač/1/rezervacije` returned HTTP 404 with message "No walker found with id 1" (route works, depends on data)

- URL: `/šetač/{walkerId}/rezervacije`  (custom attribute route)
  - Controller.Action: `BookingsController.ByWalker` (walkerId)
  - HTTP verb: GET
  - Behavior: Redirects permanently (301) to `/šetač/{walkerSlug}/rezervacije` using `RedirectToActionPermanent`.
  - Observed: tested `/šetač/1/rezervacije` (direct id route) returned HTTP 404; when walker exists it redirects to slug route

- URL: `/Payments` or `/Payments/Index`
  - Controller.Action: `PaymentsController.Index`
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Payments/Index.cshtml](Vjezba.Model/Views/Payments/Index.cshtml)
  - Observed: HTTP 200 — Title: Uplate - Šapice u pokretu

- URL: `/Payments/{id}`
  - Controller.Action: `PaymentsController.Details` (id)
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Payments/Details.cshtml](Vjezba.Model/Views/Payments/Details.cshtml)
  - Observed: HTTP 200 (for tested id=1) — Title: Detalji uplate - Šapice u pokretu

- URL: `/uplate/{year}/{month}`  (custom attribute route)
  - Controller.Action: `PaymentsController.ByPeriod` (year, month)
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Payments/Index.cshtml](Vjezba.Model/Views/Payments/Index.cshtml) (returned using `View("Index", payments)`)
  - Example: `/uplate/2026/05`
  - Observed: tested `/uplate/2026/05` returned HTTP 404 with message "No payments found for 05/2026" (route works, depends on data)

- URL: `/Reviews` or `/Reviews/Index`
  - Controller.Action: `ReviewsController.Index`
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Reviews/Index.cshtml](Vjezba.Model/Views/Reviews/Index.cshtml)
  - Observed: HTTP 200 — Title: Recenzije - Šapice u pokretu

- URL: `/Reviews/Details/{id}`
  - Controller.Action: `ReviewsController.Details` (id)
  - HTTP verb: GET
  - View: [Vjezba.Model/Views/Reviews/Details.cshtml](Vjezba.Model/Views/Reviews/Details.cshtml)


Notes:
- All controllers are registered and routed via attribute routing (`app.MapControllers()`) and the conventional default route (`{controller=Home}/{action=Index}/{id?}`) declared in [Vjezba.Model/Program.cs](Vjezba.Model/Program.cs).
- Navigation links in the layout point to the `Index` actions for the main resources and are present in [Vjezba.Model/Views/Shared/_Layout.cshtml](Vjezba.Model/Views/Shared/_Layout.cshtml).
- No partial views were detected in the Views folder; pages use the shared layout ([Vjezba.Model/Views/Shared/_Layout.cshtml](Vjezba.Model/Views/Shared/_Layout.cshtml)).
