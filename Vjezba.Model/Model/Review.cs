using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model;

public class Review
{
    [Key]
    public int Id { get; set; }

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

    [Range(typeof(decimal), "0", "5", ErrorMessage = "Ocjena mora biti između 0 i 5.")]
    [Display(Name = "Ocjena")]
    public decimal Rating { get; set; }

    [StringLength(1000, ErrorMessage = "Komentar može imati najviše 1000 znakova.")]
    [Display(Name = "Komentar")]
    public string Comment { get; set; } = string.Empty;

    [Required(ErrorMessage = "Datum je obavezan.")]
    [Display(Name = "Datum")]
    public DateTime Date { get; set; }
}
