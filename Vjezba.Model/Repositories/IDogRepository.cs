namespace Vjezba.Model.Repositories;

public interface IDogRepository
{
    IReadOnlyList<Dog> GetAll();
    IQueryable<Dog> Query();
    IReadOnlyList<Dog> GetTrackedByIds(IEnumerable<int> ids);
    Dog? GetById(int id);
    void Add(Dog dog);
    void Update(Dog dog);
    void Delete(Dog dog);
}