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
}
