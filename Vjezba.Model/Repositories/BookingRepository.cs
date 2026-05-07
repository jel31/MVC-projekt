using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Data;

namespace Vjezba.Model.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Booking> GetAll()
    {
        return _context.Bookings
            .Include(b => b.Owner)
            .Include(b => b.DogWalker)
            .Include(b => b.Dogs)
            .ThenInclude(dog => dog.Owner)
            .AsNoTracking()
            .ToList()
            .AsReadOnly();
    }
}
