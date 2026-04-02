
namespace Vjezba.Model;


public class DogWalker : User
{
    public decimal HourlyRate { get; set; }

    public List<Booking> Bookings { get; set; } = new List<Booking>();
    public List<Review> Reviews { get; set; } = new List<Review>();

    public decimal AverageRating 
    { 
        get
        {
            if (Reviews.Count == 0) return 0;
            return Reviews.Average(r => r.Rating);
        }
    }
}