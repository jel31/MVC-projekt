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

    public DogOwner? GetById(int id)
    {
        return _context.Owners
            .Include(o => o.Dogs)
            .Include(o => o.Bookings)
            .FirstOrDefault(owner => owner.Id == id);
    }

    public void Add(DogOwner owner)
    {
        _context.Owners.Add(owner);
        _context.SaveChanges();
    }

    public void Update(DogOwner owner)
    {
        _context.SaveChanges();
    }

    public void Delete(DogOwner owner)
    {
        _context.Owners.Remove(owner);
        _context.SaveChanges();
    }
}
