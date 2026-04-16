namespace Vjezba.Model.Repositories;

public class MockWalkerRepository : IWalkerRepository
{
    public IReadOnlyList<DogWalker> GetAll()
    {
        return SampleData.Walkers;
    }
}
