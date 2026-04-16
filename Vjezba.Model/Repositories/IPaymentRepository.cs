namespace Vjezba.Model.Repositories;

public interface IPaymentRepository
{
    IReadOnlyList<Payment> GetAll();
}
