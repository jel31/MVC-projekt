namespace Vjezba.Model.Repositories;

public interface IBookingRepository
{
    IReadOnlyList<Booking> GetAll();
    IQueryable<Booking> Query();
    Booking? GetById(int id);
    void Add(Booking booking);
    void Update(Booking booking);
    void Delete(Booking booking);
}
