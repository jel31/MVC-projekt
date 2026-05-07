using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Data;

namespace Vjezba.Model.Repositories;

public class DogRepository : IDogRepository
{
    private readonly ApplicationDbContext _context;

    public DogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Dog> GetAll()
    {
        return _context.Dogs
            .Include(d => d.Owner)
            .ThenInclude(owner => owner.Bookings)
            .AsNoTracking()
            .ToList()
            .AsReadOnly();
    }

    public IQueryable<Dog> Query()
    {
        return _context.Dogs
            .Include(d => d.Owner)
            .ThenInclude(owner => owner.Bookings)
            .AsNoTracking();
    }
}
