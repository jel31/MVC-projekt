namespace Vjezba.Model.Repositories;

public class MockBookingRepository : IBookingRepository
{
    public IReadOnlyList<Booking> GetAll()
    {
        return SampleData.Bookings;
    }
}
