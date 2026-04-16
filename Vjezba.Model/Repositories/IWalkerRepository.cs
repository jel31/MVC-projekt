namespace Vjezba.Model.Repositories;

public interface IWalkerRepository
{
    IReadOnlyList<DogWalker> GetAll();
}
