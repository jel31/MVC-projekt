namespace Vjezba.Model.Repositories;

public interface IDogRepository
{
    IReadOnlyList<Dog> GetAll();
}