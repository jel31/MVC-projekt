namespace Vjezba.Model.Repositories;

public interface IOwnerRepository
{
    IReadOnlyList<DogOwner> GetAll();
}