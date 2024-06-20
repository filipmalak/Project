namespace Projekt.Models;

public class IndividualCustomer : Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Pesel { get; set; }  // Unikalne, niezmienne po ustawieniu
}
