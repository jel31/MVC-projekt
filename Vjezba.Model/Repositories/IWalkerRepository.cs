namespace Vjezba.Model.Repositories;

public interface IWalkerRepository
{
    IReadOnlyList<DogWalker> GetAll();
    DogWalker? GetById(int id);
    void Add(DogWalker walker);
    void Update(DogWalker walker);
    void Delete(DogWalker walker);
}
