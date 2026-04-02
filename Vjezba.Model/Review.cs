namespace Vjezba.Model;
public class Review
{
    public int Id { get; set; }

    public required DogOwner Owner { get; set; }

    public required DogWalker DogWalker { get; set; }

    public decimal Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime Date { get; set; }
}