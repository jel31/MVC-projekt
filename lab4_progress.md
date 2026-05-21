# Lab 4 — Progress Checklist

This file tracks progress on Lab 4 tasks grouped by the requested categories.

## CRUD

- [x] Create list (Index) page with paging and display
- [x] Implement Create form and POST handler
- [x] Implement Edit form and POST handler using safe mapping / TryUpdateModel pattern
- [x] Implement Delete (consider soft delete via `DeletedAt`)
- [ ] Ensure lists and details respect business rules and relations

## AJAX Search

- [x] Add AJAX search to all list pages (as required)
- [x] Implement server-side search endpoints returning paged JSON
- [x] Debounce client input and handle empty/no-results cases
- [x] Ensure searches filter out soft-deleted records (if applicable)

## Autocomplete Dropdown

- [x] Create reusable partial view for autocomplete dropdown control
- [x] Control must use AJAX to fetch matching items from server
- [x] Store selected item `ID` in a hidden input for form submission
- [x] For Edit forms, load and display current selection (ID + display text)
- [x] Provide server endpoint that returns items in defined JSON format

## Validation

- [x] Add server-side validation attributes to models (`[Required]`, `Range`, `StringLength`, etc.)
- [x] Check `ModelState.IsValid` or use `TryUpdateModelAsync` before saving
- [x] Add client-side validation and include `_ValidationScriptsPartial` in layout
- [x] Trigger client-side validation on blur (control loses focus)
- [x] Display validation messages that integrate with the UI styles

## Animations (Advanced JavaScript)

- [x] Add animations that improve UX (e.g., smooth list updates, loading indicators)
- [x] Use unobtrusive JavaScript and keep animations accessible
- [x] Ensure animations are functional and assistive, not purely decorative

## Date Control

- [x] Create a partial view for date+time input (custom or JS plugin)
- [x] Apply the date control partial everywhere dates/times are used
- [x] Support `hr` and `en` formats based on request culture
- [x] Do NOT use the browser's native `input[type=date]`; use plugin or custom code
- [x] Validate date input on client and server and handle parsing correctly

---

Progress notes:

- Project tracking file for Lab 4 tasks.
- CRUD: Owners, Walkers, Dogs, Bookings, Payments, and Reviews CRUD flows are implemented (create/edit/delete, repository methods, controllers, views).
- AJAX search: Server endpoints added for `dogs`, `owners`, `walkers`, `bookings`, `payments`, and `reviews` in [Vjezba.Model/Controllers/SearchController.cs](Vjezba.Model/Controllers/SearchController.cs#L1). All index pages were wired to the shared client module (`/wwwroot/js/ajax-search.js`) and support debounced queries, paged results, empty states and accessible pagination.
- Soft-delete handling: Search and autocomplete queries now exclude entities with an `IsDeleted` property via a defensive `ExcludeSoftDeleted<T>` helper using the EF model metadata (applies when `IsDeleted` is present).
- Autocomplete: Reworked `AutocompleteController` to inject `ApplicationDbContext`, use the same `ExcludeSoftDeleted<T>` helper, and return paged short lists for owners/dogs/walkers/bookings. Partial view `Views/Shared/_Autocomplete.cshtml` is in use.
- Client UX: `wwwroot/js/ajax-search.js` updated with loading state, retry on error, keyboard handlers for pagination, and ARIA attributes for better accessibility.
- Animations (functional): Added loading indicator with spinner + status text during AJAX list search, smooth staggered result-card entry on new results, and busy-state announcements.
- Animations (accessible): Added reduced-motion fallbacks (no spinner rotation, no card motion) under `prefers-reduced-motion: reduce`, plus screen-reader live status updates for search state changes.
- Unobtrusive JS: All animation behavior is implemented in shared external files (`wwwroot/js/ajax-search.js`, `wwwroot/css/site.css`) without inline handlers.
- Assistive feedback: Added unobtrusive toast notifications (create/edit/delete success states) using `TempData` + shared `wwwroot/js/toast.js` + layout rendering, so users get immediate action confirmation.
- Skeleton loading: Enhanced AJAX search loading state with skeleton cards (shimmer effect with reduced-motion fallback) to improve perceived responsiveness during result fetch.
- Validation: Added and standardized server + client validation for Owners, Walkers, Dogs, Bookings, Payments, and Reviews.
- Model annotations: Added/extended validation attributes in `Model/Dog.cs`, `Model/Booking.cs`, `Model/Payment.cs`, and `Model/Review.cs`; `DogOwner`/`DogWalker` use inherited `User` validation plus walker `HourlyRate` constraints.
- Controller safety: Hardened create/edit flows to enforce `ModelState.IsValid` and safe update patterns (including async edit handlers and stronger manual checks for custom booking/payment/review form inputs).
- Blur trigger: Added `wwwroot/js/validation-blur.js` and wired it via `_ValidationScriptsPartial` so field validation is triggered on `focusout`/blur and for custom autocomplete/date controls.
- UI-integrated messages: Unified form error presentation with `validation-message` style in `wwwroot/css/forms.css` and added missing message anchors (`data-valmsg-for`) for custom controls.
- Migrations: A migration was scaffolded (`AddPendingModelChanges`) and written to `Vjezba.Model/Migrations/`. Applying the migration failed earlier when SQL Server was not available locally; starting a local SQL Server (Docker) allowed the app to run.
- Migrations: Added migration `AddValidationConstraints` after model-validation updates to keep EF model and database in sync.

Notes / Next steps:
- Verify paging and results on each index page (manual smoke test). I can run the app and exercise the search flows if you want.
- Add seed data to exercise multi-page search results and soft-delete scenarios (`SampleData` or a seed script).
- Add unit/integration tests for booking overlap, search behavior, and validation rules.



