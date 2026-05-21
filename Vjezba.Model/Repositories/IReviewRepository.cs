namespace Vjezba.Model.Repositories;

public interface IReviewRepository
{
    IReadOnlyList<Review> GetAll();
    IQueryable<Review> Query();
    Review? GetById(int id);
    void Add(Review review);
    void Update(Review review);
    void Delete(Review review);
}
