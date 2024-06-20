using Projekt.Models;

namespace Projekt.Services;

public interface IRevenueService
{
    Task<RevenueCalculation> CalculateRevenue(string currencyCode = "PLN");
    Task<RevenueCalculation> CalculateProductRevenue(int productId, string currencyCode = "PLN");
}
