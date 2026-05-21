using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model;

public class Dog
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Ime psa je obavezno.")]
    [Display(Name = "Ime")]
    [StringLength(80, ErrorMessage = "Ime može imati najviše 80 znakova.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pasmina je obavezna.")]
    [Display(Name = "Pasmina")]
    [StringLength(120, ErrorMessage = "Pasmina može imati najviše 120 znakova.")]
    public string Breed { get; set; } = string.Empty;

    [Range(0, 30, ErrorMessage = "Dob mora biti između 0 i 30 godina.")]
    [Display(Name = "Dob")]
    public int Age { get; set; }

    [Display(Name = "Cijepljen")]
    public bool IsVaccinated { get; set; }

    [Display(Name = "Prijateljski")]
    public bool IsFriendly { get; set; }

    [Required(ErrorMessage = "Vlasnik je obavezan.")]
    [Range(1, int.MaxValue, ErrorMessage = "Odaberite vlasnika.")]
    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    [ValidateNever]
    public virtual DogOwner Owner { get; set; } = null!;

    [ValidateNever]
    public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

}
