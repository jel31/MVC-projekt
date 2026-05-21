using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model;


public class Payment
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Rezervacija je obavezna.")]
    [Range(1, int.MaxValue, ErrorMessage = "Odaberite rezervaciju.")]
    public int BookingId { get; set; }

    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;

    [Range(typeof(decimal), "0.01", "999999", ErrorMessage = "Iznos mora biti veći od 0.")]
    [Display(Name = "Iznos")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Datum je obavezan.")]
    [Display(Name = "Datum")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Metoda plaćanja je obavezna.")]
    [StringLength(80, ErrorMessage = "Metoda plaćanja može imati najviše 80 znakova.")]
    [Display(Name = "Metoda plaćanja")]
    public string PaymentMethod { get; set; } = string.Empty;

    [Display(Name = "Uspješno")]
    public bool IsSuccessful { get; set; }
}
