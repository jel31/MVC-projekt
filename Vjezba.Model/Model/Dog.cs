using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model;

public class Dog
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Breed { get; set; } = string.Empty;

    public int Age { get; set; }

    public bool IsVaccinated { get; set; }

    public bool IsFriendly { get; set; }

    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public virtual DogOwner Owner { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

}
