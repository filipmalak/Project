namespace Projekt.Models;

public class Software
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public string Category { get; set; }  // Np. finanse, edukacja itp.
    public bool IsSubscriptionAvailable { get; set; }
    public bool IsSinglePurchaseAvailable { get; set; }
    public List<Discount> Discounts { get; set; }
}
