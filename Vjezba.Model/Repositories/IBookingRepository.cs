namespace Vjezba.Model.Repositories;

public interface IBookingRepository
{
    IReadOnlyList<Booking> GetAll();
}
