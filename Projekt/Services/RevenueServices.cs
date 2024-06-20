namespace Projekt.Services;

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projekt.Context;
using Projekt.Models;
using Newtonsoft.Json.Linq;

public class RevenueService : IRevenueService
{
    private readonly ApplicationDbContext _context;
    private readonly HttpClient _httpClient;

    public RevenueService(ApplicationDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    private async Task<decimal> GetExchangeRate(string currencyCode)
    {
        if (currencyCode == "PLN")
        {
            return 1m;
        }

        var response = await _httpClient.GetStringAsync($"https://api.exchangerate-api.com/v4/latest/PLN");
        var data = JObject.Parse(response);
        return data["rates"][currencyCode].Value<decimal>();
    }

    public async Task<RevenueCalculation> CalculateRevenue(string currencyCode = "PLN")
    {
        var exchangeRate = await GetExchangeRate(currencyCode);
        var totalRevenue = await _context.Payments.SumAsync(p => p.Amount) * exchangeRate;
        var totalExpectedRevenue = await _context.Contracts
            .Where(c => !c.IsPaid)
            .SumAsync(c => c.DiscountedPrice) * exchangeRate;

        return new RevenueCalculation
        {
            TotalRevenue = totalRevenue,
            TotalExpectedRevenue = totalExpectedRevenue + totalRevenue
        };
    }

    public async Task<RevenueCalculation> CalculateProductRevenue(int productId, string currencyCode = "PLN")
    {
        var exchangeRate = await GetExchangeRate(currencyCode);
        var totalRevenue = await _context.Payments
            .Where(p => p.Contract.SoftwareId == productId)
            .SumAsync(p => p.Amount) * exchangeRate;
        var totalExpectedRevenue = await _context.Contracts
            .Where(c => !c.IsPaid && c.SoftwareId == productId)
            .SumAsync(c => c.DiscountedPrice) * exchangeRate;

        return new RevenueCalculation
        {
            TotalRevenue = totalRevenue,
            TotalExpectedRevenue = totalExpectedRevenue + totalRevenue
        };
    }
}
