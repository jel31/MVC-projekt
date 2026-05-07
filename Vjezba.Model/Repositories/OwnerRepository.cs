using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Data;

namespace Vjezba.Model.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly ApplicationDbContext _context;

    public OwnerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<DogOwner> GetAll()
    {
        return _context.Owners
            .Include(o => o.Dogs)
            .Include(o => o.Bookings)
            .AsNoTracking()
            .ToList()
            .AsReadOnly();
    }
}
