using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model;


public class Payment
{
    [Key]
    public int Id { get; set; }

    public int BookingId { get; set; }

    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public bool IsSuccessful { get; set; }
}
