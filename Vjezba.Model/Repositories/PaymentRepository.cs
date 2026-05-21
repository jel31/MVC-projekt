using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Data;

namespace Vjezba.Model.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Payment> GetAll()
    {
        return _context.Payments
            .Include(p => p.Booking)
            .ThenInclude(b => b.DogWalker)
            .Include(p => p.Booking)
            .ThenInclude(b => b.Owner)
            .AsNoTracking()
            .ToList()
            .AsReadOnly();
    }

    public IQueryable<Payment> Query()
    {
        return _context.Payments
            .Include(p => p.Booking)
            .ThenInclude(b => b.DogWalker)
            .Include(p => p.Booking)
            .ThenInclude(b => b.Owner)
            .AsNoTracking();
    }

    public Payment? GetById(int id)
    {
        return _context.Payments
            .Include(p => p.Booking)
            .ThenInclude(b => b.DogWalker)
            .Include(p => p.Booking)
            .ThenInclude(b => b.Owner)
            .FirstOrDefault(p => p.Id == id);
    }

    public void Add(Payment payment)
    {
        _context.Payments.Add(payment);
        _context.SaveChanges();
    }

    public void Update(Payment payment)
    {
        _context.SaveChanges();
    }

    public void Delete(Payment payment)
    {
        _context.Payments.Remove(payment);
        _context.SaveChanges();
    }
}
