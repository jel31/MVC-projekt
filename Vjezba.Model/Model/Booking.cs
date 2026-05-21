using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model;

public class Booking
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Početak rezervacije je obavezan.")]
    [Display(Name = "Početak")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Kraj rezervacije je obavezan.")]
    [Display(Name = "Kraj")]
    public DateTime EndTime { get; set; }

    [Required(ErrorMessage = "Vlasnik je obavezan.")]
    [Range(1, int.MaxValue, ErrorMessage = "Odaberite vlasnika.")]
    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public virtual DogOwner Owner { get; set; } = null!;

    [Required(ErrorMessage = "Šetač je obavezan.")]
    [Range(1, int.MaxValue, ErrorMessage = "Odaberite šetača.")]
    public int DogWalkerId { get; set; }

    [ForeignKey(nameof(DogWalkerId))]
    public virtual DogWalker DogWalker { get; set; } = null!;

    public virtual ICollection<Dog> Dogs { get; set; } = new HashSet<Dog>();

    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

    [Display(Name = "Status")]
    public BookingStatus Status { get; set; }

}
