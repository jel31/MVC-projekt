namespace Vjezba.Model.Repositories;

public interface IPaymentRepository
{
    IReadOnlyList<Payment> GetAll();
    IQueryable<Payment> Query();
    Payment? GetById(int id);
    void Add(Payment payment);
    void Update(Payment payment);
    void Delete(Payment payment);
}
