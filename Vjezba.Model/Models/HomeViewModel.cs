namespace Vjezba.Model;

public class HomeViewModel
{
    public IReadOnlyList<DogOwner> Owners { get; set; } = Array.Empty<DogOwner>();
    public IReadOnlyList<DogWalker> Walkers { get; set; } = Array.Empty<DogWalker>();
    public IReadOnlyList<Dog> Dogs { get; set; } = Array.Empty<Dog>();
    public IReadOnlyList<Booking> Bookings { get; set; } = Array.Empty<Booking>();
    public IReadOnlyList<Payment> Payments { get; set; } = Array.Empty<Payment>();
    public IReadOnlyList<Review> Reviews { get; set; } = Array.Empty<Review>();
}
