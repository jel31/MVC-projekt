
namespace Vjezba.Model;


public class Booking
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public required DogOwner Owner { get; set; } 

    public required DogWalker DogWalker { get; set; }

    public List<Dog> Dogs { get; set; }= new List<Dog>();

    public BookingStatus Status { get; set; }

}    