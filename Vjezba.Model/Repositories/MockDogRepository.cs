namespace Vjezba.Model.Repositories;

public class MockDogRepository : IDogRepository
{
    public IReadOnlyList<Dog> GetAll()
    {
        return SampleData.Dogs;
    }
}