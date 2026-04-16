namespace Vjezba.Model.Repositories;

public interface IReviewRepository
{
    IReadOnlyList<Review> GetAll();
}