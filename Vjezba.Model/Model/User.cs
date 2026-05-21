using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model;


public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Ovo polje je obavezno.")]
    [Display(Name = "Ime")]
    [StringLength(100, ErrorMessage = "Najviše 100 znakova.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ovo polje je obavezno.")]
    [Display(Name = "Prezime")]
    [StringLength(100, ErrorMessage = "Najviše 100 znakova.")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ovo polje je obavezno.")]
    [Display(Name = "E-pošta")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu e-mail adresu.")]
    [StringLength(150, ErrorMessage = "Najviše 150 znakova.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ovo polje je obavezno.")]
    [Display(Name = "Telefon")]
    [StringLength(30, ErrorMessage = "Najviše 30 znakova.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ovo polje je obavezno.")]
    [Display(Name = "Adresa")]
    [StringLength(200, ErrorMessage = "Najviše 200 znakova.")]
    public string Address { get; set; } = string.Empty;

}
