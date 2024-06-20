namespace Projekt.Models;

public class CompanyCustomer : Customer
{
    public string CompanyName { get; set; }
    public string Krs { get; set; }  // Unikalne, niezmienne po ustawieniu
}
