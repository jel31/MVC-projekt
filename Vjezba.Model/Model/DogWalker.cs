using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Vjezba.Model;


public class DogWalker : User
{
    [Range(0.01, 1000, ErrorMessage = "Unesite valjanu satnicu.")]
    [Display(Name = "Cijena po satu")]
    public decimal HourlyRate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

    public decimal AverageRating 
    { 
        get
        {
            if (Reviews == null || Reviews.Count == 0) return 0;
            return Reviews.Average(r => r.Rating);
        }
    }
}
