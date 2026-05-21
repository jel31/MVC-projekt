using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Data;

namespace Vjezba.Model.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _context;

    public ReviewRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Review> GetAll()
    {
        return _context.Reviews
            .Include(r => r.Owner)
            .Include(r => r.DogWalker)
            .AsNoTracking()
            .ToList()
            .AsReadOnly();
    }

    public IQueryable<Review> Query()
    {
        return _context.Reviews
            .Include(r => r.Owner)
            .Include(r => r.DogWalker)
            .AsNoTracking();
    }

    public Review? GetById(int id)
    {
        return _context.Reviews
            .Include(r => r.Owner)
            .Include(r => r.DogWalker)
            .FirstOrDefault(r => r.Id == id);
    }

    public void Add(Review review)
    {
        _context.Reviews.Add(review);
        _context.SaveChanges();
    }

    public void Update(Review review)
    {
        _context.SaveChanges();
    }

    public void Delete(Review review)
    {
        _context.Reviews.Remove(review);
        _context.SaveChanges();
    }
}
