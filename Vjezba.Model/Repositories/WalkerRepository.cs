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
            .AsNoTracking()
            .ToList()
            .AsReadOnly();
    }
}
