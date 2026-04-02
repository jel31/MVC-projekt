
namespace Vjezba.Model;


public class Payment
{
    public int Id { get; set; }

    public required Booking Booking { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public bool IsSuccessful { get; set; }
}