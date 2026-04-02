namespace Vjezba.Model;


public class DogOwner : User
{
    public List<Dog> Dogs { get; set; } = new List<Dog>();

    public List<Booking> Bookings { get; set; } = new List<Booking>();

    

}