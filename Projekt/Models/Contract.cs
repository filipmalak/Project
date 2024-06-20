namespace Projekt.Models;

public class Contract
{
    public int Id { get; set; }
    public int SoftwareId { get; set; }
    public Software Software { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal BasePrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public bool IsPaid { get; set; }
    public int SupportYears { get; set; }  // 0, 1, 2, 3 lata dodatkowego wsparcia
    public List<Payment> Payments { get; set; }
}
