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

    public IReadOnlyList<Dog> GetTrackedByIds(IEnumerable<int> ids)
    {
        var dogIds = ids.Distinct().ToList();
        if (dogIds.Count == 0)
            return Array.Empty<Dog>();

        return _context.Dogs
            .Where(d => dogIds.Contains(d.Id))
            .ToList();
    }

    public Dog? GetById(int id)
    {
        return _context.Dogs
            .Include(d => d.Owner)
            .Include(d => d.Bookings)
            .FirstOrDefault(d => d.Id == id);
    }

    public void Add(Dog dog)
    {
        _context.Dogs.Add(dog);
        _context.SaveChanges();
    }

    public void Update(Dog dog)
    {
        _context.SaveChanges();
    }

    public void Delete(Dog dog)
    {
        _context.Dogs.Remove(dog);
        _context.SaveChanges();
    }
}
