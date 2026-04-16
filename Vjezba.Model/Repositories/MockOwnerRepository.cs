namespace Vjezba.Model.Repositories;

public class MockOwnerRepository : IOwnerRepository
{
    public IReadOnlyList<DogOwner> GetAll()
    {
        return SampleData.Owners;
    }
}
