namespace Vjezba.Model.Repositories;

public class MockReviewRepository : IReviewRepository
{
    public IReadOnlyList<Review> GetAll()
    {
        return SampleData.Reviews;
    }
}