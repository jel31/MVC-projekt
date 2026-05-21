using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Data;

namespace Vjezba.Model.Repositories;

public class WalkerRepository : IWalkerRepository
{
    private readonly ApplicationDbContext _context;

    public WalkerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<DogWalker> GetAll()
    {
        return _context.Walkers
            .Include(w => w.Bookings)
            .ThenInclude(booking => booking.Owner)
            .Include(w => w.Reviews)
            .ThenInclude(review => review.Owner)
            .AsNoTracking()
            .ToList()
            .AsReadOnly();
    }

    public DogWalker? GetById(int id)
    {
        return _context.Walkers
            .Include(w => w.Bookings)
            .ThenInclude(booking => booking.Owner)
            .Include(w => w.Reviews)
            .ThenInclude(review => review.Owner)
            .FirstOrDefault(walker => walker.Id == id);
    }

    public void Add(DogWalker walker)
    {
        _context.Walkers.Add(walker);
        _context.SaveChanges();
    }

    public void Update(DogWalker walker)
    {
        _context.SaveChanges();
    }

    public void Delete(DogWalker walker)
    {
        _context.Walkers.Remove(walker);
        _context.SaveChanges();
    }
}
