using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model;

public class Booking
{
    [Key]
    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public virtual DogOwner Owner { get; set; } = null!;

    public int DogWalkerId { get; set; }

    [ForeignKey(nameof(DogWalkerId))]
    public virtual DogWalker DogWalker { get; set; } = null!;

    public virtual ICollection<Dog> Dogs { get; set; } = new HashSet<Dog>();

    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

    public BookingStatus Status { get; set; }

}
