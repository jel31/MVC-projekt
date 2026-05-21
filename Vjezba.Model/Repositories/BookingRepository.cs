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

    public IQueryable<Booking> Query()
    {
        return _context.Bookings
            .Include(b => b.Owner)
            .Include(b => b.DogWalker)
            .Include(b => b.Dogs)
            .ThenInclude(dog => dog.Owner)
            .AsNoTracking();
    }

    public Booking? GetById(int id)
    {
        return _context.Bookings
            .Include(b => b.Owner)
            .Include(b => b.DogWalker)
            .Include(b => b.Dogs)
            .ThenInclude(d => d.Owner)
            .FirstOrDefault(b => b.Id == id);
    }

    public void Add(Booking booking)
    {
        _context.Bookings.Add(booking);
        _context.SaveChanges();
    }

    public void Update(Booking booking)
    {
        _context.SaveChanges();
    }

    public void Delete(Booking booking)
    {
        _context.Bookings.Remove(booking);
        _context.SaveChanges();
    }
}
