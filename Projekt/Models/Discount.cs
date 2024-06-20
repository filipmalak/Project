namespace Projekt.Models;

public class Discount
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SoftwareId { get; set; }
    public Software Software { get; set; }
}
