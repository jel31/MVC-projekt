namespace Vjezba.Model.Repositories;

public interface IOwnerRepository
{
    IReadOnlyList<DogOwner> GetAll();
    DogOwner? GetById(int id);
    void Add(DogOwner owner);
    void Update(DogOwner owner);
    void Delete(DogOwner owner);
}