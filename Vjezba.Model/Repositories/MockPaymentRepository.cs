namespace Vjezba.Model.Repositories;

public class MockPaymentRepository : IPaymentRepository
{
    public IReadOnlyList<Payment> GetAll()
    {
        return SampleData.Payments;
    }
}
