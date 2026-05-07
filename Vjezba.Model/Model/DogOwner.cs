using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model;


public class DogOwner : User
{
    public virtual ICollection<Dog> Dogs { get; set; } = new HashSet<Dog>();

    public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

}
