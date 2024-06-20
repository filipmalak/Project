namespace Projekt.Models;

public abstract class Customer
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsDeleted { get; set; } = false;
}
