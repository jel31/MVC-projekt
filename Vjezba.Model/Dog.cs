 namespace Vjezba.Model;
public class Dog
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Breed { get; set; } = string.Empty;

    public int Age { get; set; }

    public bool IsVaccinated { get; set; }

    public bool IsFriendly { get; set; }

    public required DogOwner Owner { get; set; }


    
}