using System.Linq;
using Vjezba.Model.Data;

namespace Vjezba.Model;

public static class SampleData
{
    public static List<DogOwner> Owners { get; } = new List<DogOwner>();
    public static List<DogWalker> Walkers { get; } = new List<DogWalker>();
    public static List<Dog> Dogs { get; } = new List<Dog>();
    public static List<Booking> Bookings { get; } = new List<Booking>();
    public static List<Payment> Payments { get; } = new List<Payment>();
    public static List<Review> Reviews { get; } = new List<Review>();

    static SampleData()
    {
        var owner1 = new DogOwner
        {
            Name = "Ana",
            Surname = "Horvat",
            Email = "ana.horvat@example.com",
            PhoneNumber = "0912345678",
            Address = "Zagreb"
        };

        var owner2 = new DogOwner
        {
            Name = "Petra",
            Surname = "Kovac",
            Email = "petra.kovac@example.com",
            PhoneNumber = "0923456789",
            Address = "Zagreb"
        };

        var owner3 = new DogOwner
        {
            Name = "Ivan",
            Surname = "Babic",
            Email = "ivan.babic@example.com",
            PhoneNumber = "0934567890",
            Address = "Zagreb"
        };

        var walker1 = new DogWalker
        {
            Name = "Marko",
            Surname = "Ivic",
            Email = "marko.ivic@example.com",
            PhoneNumber = "0987654321",
            Address = "Zagreb",
            HourlyRate = 15m
        };

        var walker2 = new DogWalker
        {
            Name = "Luka",
            Surname = "Maric",
            Email = "luka.maric@example.com",
            PhoneNumber = "0976543210",
            Address = "Zagreb",
            HourlyRate = 12m
        };

        var dog1 = new Dog
        {
            Name = "Rex",
            Breed = "German Shepherd",
            Age = 3,
            IsVaccinated = true,
            IsFriendly = true,
            Owner = owner1
        };

        var dog2 = new Dog
        {
            Name = "Bella",
            Breed = "Labrador",
            Age = 2,
            IsVaccinated = true,
            IsFriendly = true,
            Owner = owner1
        };

        var dog3 = new Dog
        {
            Name = "Max",
            Breed = "Beagle",
            Age = 5,
            IsVaccinated = false,
            IsFriendly = true,
            Owner = owner2
        };

        var dog4 = new Dog
        {
            Name = "Luna",
            Breed = "Golden Retriever",
            Age = 1,
            IsVaccinated = true,
            IsFriendly = true,
            Owner = owner3
        };

        var dog5 = new Dog
        {
            Name = "Bruno",
            Breed = "Labrador",
            Age = 4,
            IsVaccinated = true,
            IsFriendly = false,
            Owner = owner3
        };

        var dog6 = new Dog
        {
            Name = "Coco",
            Breed = "Poodle",
            Age = 3,
            IsVaccinated = true,
            IsFriendly = true,
            Owner = owner3
        };

        owner1.Dogs.Add(dog1);
        owner1.Dogs.Add(dog2);
        owner2.Dogs.Add(dog3);
        owner3.Dogs.Add(dog4);
        owner3.Dogs.Add(dog5);
        owner3.Dogs.Add(dog6);

        var booking1 = new Booking
        {
            StartTime = new DateTime(2026, 3, 3, 9, 0, 0),
            EndTime = new DateTime(2026, 3, 3, 10, 0, 0),
            Owner = owner1,
            DogWalker = walker1,
            Status = BookingStatus.Completed,
            Dogs = new List<Dog> { dog1, dog2 }
        };

        var booking2 = new Booking
        {
            StartTime = new DateTime(2026, 3, 3, 11, 0, 0),
            EndTime = new DateTime(2026, 3, 3, 12, 0, 0),
            Owner = owner2,
            DogWalker = walker1,
            Status = BookingStatus.Completed,
            Dogs = new List<Dog> { dog3 }
        };

        var booking3 = new Booking
        {
            StartTime = new DateTime(2026, 3, 3, 8, 0, 0),
            EndTime = new DateTime(2026, 3, 3, 9, 30, 0),
            Owner = owner1,
            DogWalker = walker2,
            Status = BookingStatus.Completed,
            Dogs = new List<Dog> { dog1 }
        };

        var booking4 = new Booking
        {
            StartTime = new DateTime(2026, 3, 3, 10, 0, 0),
            EndTime = new DateTime(2026, 3, 3, 11, 30, 0),
            Owner = owner3,
            DogWalker = walker2,
            Status = BookingStatus.Completed,
            Dogs = new List<Dog> { dog4, dog5, dog6 }
        };

        owner1.Bookings.Add(booking1);
        owner1.Bookings.Add(booking3);
        owner2.Bookings.Add(booking2);
        owner3.Bookings.Add(booking4);
        walker1.Bookings.Add(booking1);
        walker1.Bookings.Add(booking2);
        walker2.Bookings.Add(booking3);
        walker2.Bookings.Add(booking4);

        var payment1 = new Payment
        {
            Booking = booking1,
            Amount = (decimal)(booking1.EndTime - booking1.StartTime).TotalHours * walker1.HourlyRate,
            Date = booking1.EndTime.AddMinutes(30),
            PaymentMethod = "Credit Card",
            IsSuccessful = true
        };

        var payment2 = new Payment
        {
            Booking = booking2,
            Amount = (decimal)(booking2.EndTime - booking2.StartTime).TotalHours * walker1.HourlyRate,
            Date = booking2.EndTime.AddMinutes(30),
            PaymentMethod = "Cash",
            IsSuccessful = true
        };

        var payment3 = new Payment
        {
            Booking = booking3,
            Amount = (decimal)(booking3.EndTime - booking3.StartTime).TotalHours * walker2.HourlyRate,
            Date = booking3.EndTime.AddMinutes(30),
            PaymentMethod = "Credit Card",
            IsSuccessful = true
        };

        var payment4 = new Payment
        {
            Booking = booking4,
            Amount = (decimal)(booking4.EndTime - booking4.StartTime).TotalHours * walker2.HourlyRate,
            Date = booking4.EndTime.AddMinutes(30),
            PaymentMethod = "Bank Transfer",
            IsSuccessful = false
        };

        var review1 = new Review
        {
            Owner = owner1,
            DogWalker = walker1,
            Rating = 4.8m,
            Comment = "Marko was punctual and handled both dogs very well.",
            Date = booking1.EndTime.AddHours(2)
        };

        var review2 = new Review
        {
            Owner = owner1,
            DogWalker = walker2,
            Rating = 4.6m,
            Comment = "Luka gave Rex a calm walk and shared a helpful update afterward.",
            Date = booking3.EndTime.AddHours(2)
        };

        walker1.Reviews.Add(review1);
        walker2.Reviews.Add(review2);

        Owners.Add(owner1);
        Owners.Add(owner2);
        Owners.Add(owner3);
        Walkers.Add(walker1);
        Walkers.Add(walker2);
        Dogs.AddRange(new[] { dog1, dog2, dog3, dog4, dog5, dog6 });
        Bookings.AddRange(new[] { booking1, booking2, booking3, booking4 });
        Payments.AddRange(new[] { payment1, payment2, payment3, payment4 });
        Reviews.AddRange(new[] { review1, review2 });
    }

    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Owners.Any())
            return;  // Database already seeded

        context.Owners.AddRange(Owners);
        context.Walkers.AddRange(Walkers);
        context.Dogs.AddRange(Dogs);
        context.Bookings.AddRange(Bookings);
        context.Payments.AddRange(Payments);
        context.Reviews.AddRange(Reviews);
        context.SaveChanges();
    }
}
