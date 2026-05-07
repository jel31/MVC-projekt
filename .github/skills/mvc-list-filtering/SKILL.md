---
name: mvc-list-filtering
description: "Use when adding or updating filtering on ASP.NET Core MVC list or index pages. Covers FilterViewModel, reusable partial views, HTTP GET query-string binding, EF Core LINQ filtering, and strongly typed Razor views."
user-invocable: true
---

# MVC List Filtering

Use this skill when a contributor needs to add filtering to an existing ASP.NET Core MVC list or index page.

This skill is intentionally narrow. It focuses on one repeatable pattern:

- a dedicated `FilterViewModel`
- a reusable partial view for the filter UI
- HTTP GET query-string filtering
- EF Core LINQ filtering before materializing results
- strongly typed Razor views for both the filter form and the results

## Inputs

- Existing MVC controller action and repository method for the list page.
- The entity list view that needs filtering.
- The filter fields required by the page.

## Outputs

- A page-specific `FilterViewModel`.
- A page-specific `IndexPageViewModel` that combines items and filter state.
- A GET-based controller `Index` action that reads query-string values.
- A reusable `_EntityFilter.cshtml` partial view.
- A strongly typed `Index.cshtml` that renders the partial and the filtered results.

## When to Use

- The page already shows a list of entities and needs filtering.
- The filter should be shareable, bookmarkable, and refresh-safe.
- The project uses controllers, Razor views, and repository-based data access.

## When Not to Use

- The task is only about sorting, paging, or exporting.
- The task needs complex multi-step workflows better handled by a form POST.
- The page is not a list or index view.

## Procedure

1. Inspect the current controller, repository, and index view.
2. Identify the filter fields that matter for the page.
3. Create a dedicated `FilterViewModel` for those fields only.
4. Update the index action to accept the filter model through the query string.
5. Apply EF Core filtering in the query pipeline before `ToList()` or similar materialization.
6. Keep the results view strongly typed and wrap the page in a composite model if the page needs both items and filter state.
7. Extract the filter UI into a reusable partial view.
8. Use `method="get"` so filter values appear in the URL.
9. Add a reset link that clears the query string.
10. Verify that the page still renders correctly when no filters are supplied.

## Design Rules

- Prefer GET for filtering because it makes URLs shareable and debuggable.
- Do not filter in memory unless the dataset is tiny and the repository cannot expose a queryable source.
- Keep the filter model minimal and page-specific.
- Use a reusable partial only for the form chrome and common controls; keep page-specific labels and options in the page model.
- Keep the Razor view strongly typed. Avoid `ViewBag` for filter state.

## Common Pitfalls

- The partial view is given the wrong model type, which breaks tag helpers or loses selected values.
- The controller filters after materializing the query, which moves work into memory and can hurt performance.
- The filter form uses POST, which makes the page harder to share and bookmark.
- The index view binds to the entity list directly instead of a page view model, which makes it difficult to preserve filter state.

## Pattern Reference

Embed the pattern below directly in the skill so contributors do not have to open another file.

### FilterViewModel

```csharp
public sealed class FilterViewModel
{
	public string? SearchTerm { get; set; }
	public bool? IsActive { get; set; }
	public int? MinAge { get; set; }
	public int? MaxAge { get; set; }
}
```

### IndexPageViewModel

```csharp
public sealed class IndexPageViewModel
{
	public IReadOnlyList<Entity> Items { get; init; } = [];
	public FilterViewModel Filter { get; init; } = new();
}
```

### Controller Index Action Using GET Query String

```csharp
[HttpGet]
public IActionResult Index([FromQuery] FilterViewModel filter)
{
	var query = _repository.Query();

	if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
	{
		query = query.Where(entity =>
			entity.Name.Contains(filter.SearchTerm) ||
			entity.Description.Contains(filter.SearchTerm));
	}

	if (filter.IsActive.HasValue)
	{
		query = query.Where(entity => entity.IsActive == filter.IsActive.Value);
	}

	if (filter.MinAge.HasValue)
	{
		query = query.Where(entity => entity.Age >= filter.MinAge.Value);
	}

	if (filter.MaxAge.HasValue)
	{
		query = query.Where(entity => entity.Age <= filter.MaxAge.Value);
	}

	var items = query
		.OrderBy(entity => entity.Name)
		.ToList();

	var viewModel = new IndexPageViewModel
	{
		Items = items,
		Filter = filter
	};

	return View(viewModel);
}
```

### EF Repository Filtering With Conditional Where

```csharp
public IQueryable<Entity> Query()
{
	return _context.Entities.AsNoTracking();
}

public IQueryable<Entity> ApplyFilter(IQueryable<Entity> query, FilterViewModel filter)
{
	if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
	{
		query = query.Where(entity =>
			entity.Name.Contains(filter.SearchTerm) ||
			entity.Description.Contains(filter.SearchTerm));
	}

	if (filter.IsActive.HasValue)
	{
		query = query.Where(entity => entity.IsActive == filter.IsActive.Value);
	}

	if (filter.MinAge.HasValue)
	{
		query = query.Where(entity => entity.Age >= filter.MinAge.Value);
	}

	if (filter.MaxAge.HasValue)
	{
		query = query.Where(entity => entity.Age <= filter.MaxAge.Value);
	}

	return query;
}
```

### _EntityFilter.cshtml Using Tag Helpers

```cshtml
@model FilterViewModel

<form method="get" class="filter-bar">
	<div class="filter-field">
		<label asp-for="SearchTerm"></label>
		<input asp-for="SearchTerm" class="form-control" />
	</div>

	<div class="filter-field">
		<label asp-for="IsActive"></label>
		<select asp-for="IsActive" class="form-select">
			<option value="">All</option>
			<option value="true">Active</option>
			<option value="false">Inactive</option>
		</select>
	</div>

	<div class="filter-field">
		<label asp-for="MinAge"></label>
		<input asp-for="MinAge" type="number" class="form-control" />
	</div>

	<div class="filter-field">
		<label asp-for="MaxAge"></label>
		<input asp-for="MaxAge" type="number" class="form-control" />
	</div>

	<div class="filter-actions">
		<button type="submit" class="btn btn-primary">Filter</button>
		<a asp-action="Index" class="btn btn-outline-secondary">Reset</a>
	</div>
</form>
```

### Index.cshtml Showing <partial .../>

```cshtml
@model IndexPageViewModel

<partial name="_EntityFilter" model="Model.Filter" />

@if (!Model.Items.Any())
{
	<p>No results found.</p>
}
else
{
	foreach (var item in Model.Items)
	{
		<div>@item.Name</div>
	}
}
```
