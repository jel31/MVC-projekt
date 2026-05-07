namespace Vjezba.Model.Models;

public sealed class DogIndexPageViewModel
{
    public IReadOnlyList<Dog> Items { get; init; } = [];
    public DogFilterViewModel Filter { get; init; } = new();
}
