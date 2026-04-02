namespace Vjezba.Model;

public class Program
{
    public static void Main(string[] args)
    {
        // --- Owners ---
        var owner1 = new DogOwner
        {
            Id = 1,
            Name = "Ana",
            Surname = "Horvat",
            Email = "ana.horvat@example.com",
            PhoneNumber = "0912345678",
            Address = "Zagreb"
        };

        var owner2 = new DogOwner
        {
            Id = 2,
            Name = "Petra",
            Surname = "Kovac",
            Email = "petra.kovac@example.com",
            PhoneNumber = "0923456789",
            Address = "Zagreb"
        };

        var owner3 = new DogOwner
        {
            Id = 5,
            Name = "Ivan",
            Surname = "Babic",
            Email = "ivan.babic@example.com",
            PhoneNumber = "0934567890",
            Address = "Zagreb"
        };

        // --- Walkers ---
        var walker1 = new DogWalker
        {
            Id = 3,
            Name = "Marko",
            Surname = "Ivic",
            Email = "marko.ivic@example.com",
            PhoneNumber = "0987654321",
            Address = "Zagreb",
            HourlyRate = 15m
        };

        var walker2 = new DogWalker
        {
            Id = 4,
            Name = "Luka",
            Surname = "Maric",
            Email = "luka.maric@example.com",
            PhoneNumber = "0976543210",
            Address = "Zagreb",
            HourlyRate = 12m
        };

        // --- Dogs ---
        var dog1 = new Dog
        {
            Id = 1,
            Name = "Rex",
            Breed = "German Shepherd",
            Age = 3,
            IsVaccinated = true,
            IsFriendly = true,
            Owner = owner1
        };

        var dog2 = new Dog
        {
            Id = 2,
            Name = "Bella",
            Breed = "Labrador",
            Age = 2,
            IsVaccinated = true,
            IsFriendly = true,
            Owner = owner1
        };

        var dog3 = new Dog
        {
            Id = 3,
            Name = "Max",
            Breed = "Beagle",
            Age = 5,
            IsVaccinated = false,
            IsFriendly = true,
            Owner = owner2
        };

        var dog4 = new Dog
        {
            Id = 4,
            Name = "Luna",
            Breed = "Golden Retriever",
            Age = 1,
            IsVaccinated = true,
            IsFriendly = true,
            Owner = owner3
        };

        var dog5 = new Dog
        {
            Id = 5,
            Name = "Bruno",
            Breed = "Labrador",
            Age = 4,
            IsVaccinated = true,
            IsFriendly = false,
            Owner = owner3
        };

        var dog6 = new Dog
        {
            Id = 6,
            Name = "Coco",
            Breed = "Poodle",
            Age = 3,
            IsVaccinated = true,
            IsFriendly = true,
            Owner = owner3
        };

        owner1.Dogs.AddRange(new[] { dog1, dog2 });
        owner2.Dogs.Add(dog3);
        owner3.Dogs.AddRange(new[] { dog4, dog5, dog6 });

        // --- Booking 1: owner1 (Rex + Bella) with walker1 ---
        var booking1 = new Booking
        {
            Id = 1,
            StartTime = new DateTime(2026, 3, 3, 9, 0, 0),
            EndTime = new DateTime(2026, 3, 3, 10, 0, 0),
            Owner = owner1,
            DogWalker = walker1,
            Status = BookingStatus.Completed,
            Dogs = new List<Dog> { dog1, dog2 }
        };

        // --- Booking 2: owner2 (Max) with walker1 ---
        var booking2 = new Booking
        {
            Id = 2,
            StartTime = new DateTime(2026, 3, 3, 11, 0, 0),
            EndTime = new DateTime(2026, 3, 3, 12, 0, 0),
            Owner = owner2,
            DogWalker = walker1,
            Status = BookingStatus.Completed,
            Dogs = new List<Dog> { dog3 }
        };

        // --- Booking 3: owner1 (Rex) with walker2 ---
        var booking3 = new Booking
        {
            Id = 3,
            StartTime = new DateTime(2026, 3, 3, 8, 0, 0),
            EndTime = new DateTime(2026, 3, 3, 9, 30, 0),
            Owner = owner1,
            DogWalker = walker2,
            Status = BookingStatus.Completed,
            Dogs = new List<Dog> { dog1 }
        };

        // --- Booking 4: owner3 (Luna + Bruno + Coco) with walker2 ---
        var booking4 = new Booking
        {
            Id = 4,
            StartTime = new DateTime(2026, 3, 3, 10, 0, 0),
            EndTime = new DateTime(2026, 3, 3, 11, 30, 0),
            Owner = owner3,
            DogWalker = walker2,
            Status = BookingStatus.Completed,
            Dogs = new List<Dog> { dog4, dog5, dog6 }
        };

        owner1.Bookings.AddRange(new[] { booking1, booking3 });
        owner2.Bookings.Add(booking2);
        owner3.Bookings.Add(booking4);
        walker1.Bookings.AddRange(new[] { booking1, booking2 });
        walker2.Bookings.AddRange(new[] { booking3, booking4 });

        // --- Payments ---
        static decimal CalcAmount(Booking b) =>
            (decimal)(b.EndTime - b.StartTime).TotalHours * b.DogWalker.HourlyRate;

        var payment1 = new Payment
        {
            Id = 1,
            Booking = booking1,
            Amount = CalcAmount(booking1),                        // 1 hr × €15 = €15.00
            Date = booking1.EndTime.AddMinutes(30),
            PaymentMethod = "Credit Card",
            IsSuccessful = true
        };

        var payment2 = new Payment
        {
            Id = 2,
            Booking = booking2,
            Amount = CalcAmount(booking2),                        // 1 hr × €15 = €15.00
            Date = booking2.EndTime.AddMinutes(30),
            PaymentMethod = "Cash",
            IsSuccessful = true
        };

        var payment3 = new Payment
        {
            Id = 3,
            Booking = booking3,
            Amount = CalcAmount(booking3),                        // 1.5 hr × €12 = €18.00
            Date = booking3.EndTime.AddMinutes(30),
            PaymentMethod = "Credit Card",
            IsSuccessful = true
        };

        var payment4 = new Payment
        {
            Id = 4,
            Booking = booking4,
            Amount = CalcAmount(booking4),                        // 1.5 hr × €12 = €18.00
            Date = booking4.EndTime.AddMinutes(30),
            PaymentMethod = "Bank Transfer",
            IsSuccessful = false
        };

        var payments = new[] { payment1, payment2, payment3, payment4 };

        // --- Reviews: only owners who already had bookings with the walker ---
        var review1 = new Review
        {
            Id = 1,
            Owner = owner1,
            DogWalker = walker1,
            Rating = 4.8m,
            Comment = "Marko was punctual and handled both dogs very well.",
            Date = booking1.EndTime.AddHours(2)
        };

        var review2 = new Review
        {
            Id = 2,
            Owner = owner1,
            DogWalker = walker2,
            Rating = 4.6m,
            Comment = "Luka gave Rex a calm walk and shared a helpful update afterward.",
            Date = booking3.EndTime.AddHours(2)
        };

        walker1.Reviews.Add(review1);
        walker2.Reviews.Add(review2);

        // --- LINQ: completed bookings ---
        var allBookings = new List<Booking> { booking1, booking2, booking3, booking4 };

        var completedBookings = allBookings
            .Where(b => b.Status == BookingStatus.Completed)
            .ToList();

        Console.WriteLine("=== Completed Bookings ===\n");
        foreach (var booking in completedBookings)
        {
            Console.WriteLine($"Booking #{booking.Id} — {booking.Owner.Name} with dog walker {booking.DogWalker.Name} on {booking.StartTime:dd.MM.yyyy}");
        }
        Console.WriteLine();

        // --- LINQ: owners with exactly one dog ---
        var allOwners = new List<DogOwner> { owner1, owner2, owner3 };

        var ownersWithOneDog = allOwners
            .Where(o => o.Dogs.Count == 1)
            .ToList();

        Console.WriteLine("=== Owners With One Dog ===\n");
        foreach (var owner in ownersWithOneDog)
        {
            Console.WriteLine($"{owner.Name} {owner.Surname} — {owner.Dogs[0].Name} ({owner.Dogs[0].Breed})");
        }
        Console.WriteLine();

        // --- LINQ: dog walkers ordered by hourly rate descending ---
        var allWalkers = new List<DogWalker> { walker1, walker2 };

        var walkersByRate = allWalkers
            .OrderByDescending(w => w.HourlyRate)
            .ToList();

        Console.WriteLine("=== Dog Walkers by Hourly Rate (Highest to Lowest) ===\n");
        foreach (var walker in walkersByRate)
        {
            Console.WriteLine($"{walker.Name} {walker.Surname} — €{walker.HourlyRate}/hr");
        }
        Console.WriteLine();

        // --- LINQ: total earnings per walker from completed bookings with successful payments ---
        var earnings = allWalkers
            .Select(w => new
            {
                Walker = w,
                TotalEarnings = payments
                    .Where(p => p.Booking.DogWalker.Id == w.Id
                             && p.Booking.Status == BookingStatus.Completed
                             && p.IsSuccessful)
                    .Sum(p => p.Amount)
            })
            .ToList();

        Console.WriteLine("=== Walker Earnings (Completed + Paid) ===\n");
        foreach (var entry in earnings)
        {
            Console.WriteLine($"{entry.Walker.Name} {entry.Walker.Surname} — €{entry.TotalEarnings:F2}");
        }
        Console.WriteLine();

        // --- LINQ: all bookings for Marko ---
        var markoBookings = allBookings
            .Where(b => b.DogWalker.Name == "Marko" && b.DogWalker.Surname == "Ivic")
            .ToList();

        Console.WriteLine("=== Marko's Bookings ===\n");
        foreach (var booking in markoBookings)
        {
            Console.WriteLine($"Booking #{booking.Id} [{booking.Status}] — {booking.Owner.Name} {booking.Owner.Surname} on {booking.StartTime:dd.MM.yyyy HH:mm}");
        }
        Console.WriteLine();

        // --- LINQ: friendly dogs — predicate is the lambda b => b.IsFriendly ---
        var allDogs = new List<Dog> { dog1, dog2, dog3, dog4, dog5, dog6 };

        var friendlyDogs = allDogs
            .Where(d => d.IsFriendly)
            .ToList();

        Console.WriteLine("=== Friendly Dogs ===\n");
        foreach (var dog in friendlyDogs)
        {
            Console.WriteLine($"{dog.Name} ({dog.Breed}) — Owner: {dog.Owner.Name} {dog.Owner.Surname}");
        }
        Console.WriteLine();

        // --- LINQ: distinct dog breeds ---
        var distinctBreeds = allDogs
            .Select(d => d.Breed)
            .Distinct()
            .ToList();

        Console.WriteLine("=== Distinct Dog Breeds ===\n");
        foreach (var breed in distinctBreeds)
        {
            Console.WriteLine(breed);
        }
        Console.WriteLine();

        // --- Print summary ---
        Console.WriteLine("=== Dog Walking Bookings ===\n");

        foreach (var booking in new[] { booking1, booking2, booking3, booking4 })
        {
            var payment = Array.Find(payments, p => p.Booking.Id == booking.Id);
            Console.WriteLine($"Booking #{booking.Id} [{booking.Status}]");
            Console.WriteLine($"  Owner:    {booking.Owner.Name} {booking.Owner.Surname}");
            Console.WriteLine($"  Walker:   {booking.DogWalker.Name} {booking.DogWalker.Surname} (€{booking.DogWalker.HourlyRate}/hr)");
            Console.WriteLine($"  Dogs:     {string.Join(", ", booking.Dogs.Select(d => d.Name))}");
            Console.WriteLine($"  Time:     {booking.StartTime:HH:mm} - {booking.EndTime:HH:mm} on {booking.StartTime:dd.MM.yyyy}");
            if (payment != null)
            {
                Console.WriteLine($"  Payment:  €{payment.Amount:F2} via {payment.PaymentMethod} on {payment.Date:dd.MM.yyyy HH:mm} [{(payment.IsSuccessful ? "Paid" : "Failed")}]");
            }
            Console.WriteLine();
        }
    }
}
