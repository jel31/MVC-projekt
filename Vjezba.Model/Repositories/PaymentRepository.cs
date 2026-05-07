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
            .ThenInclude(booking => booking.Owner)
            .AsNoTracking()
            .ToList()
            .AsReadOnly();
    }
}
