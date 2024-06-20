
namespace Projekt.Tests;

using Moq;
using Xunit;
using Projekt.Context;
using Projekt.Models;
using Projekt.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using Moq.Protected;
using System.Threading;

public class RevenueServiceTests
{
    private async Task<ApplicationDbContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "RevenueDatabase")
            .Options;

        var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();

        context.Payments.AddRange(new List<Payment>
        {
            new Payment { Id = 1, ContractId = 1, Amount = 100, PaymentDate = DateTime.UtcNow },
            new Payment { Id = 2, ContractId = 2, Amount = 200, PaymentDate = DateTime.UtcNow }
        });

        context.Contracts.AddRange(new List<Contract>
        {
            new Contract { Id = 1, SoftwareId = 1, CustomerId = 1, BasePrice = 300, DiscountedPrice = 250, IsPaid = true, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) },
            new Contract { Id = 2, SoftwareId = 2, CustomerId = 2, BasePrice = 500, DiscountedPrice = 400, IsPaid = false, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) }
        });

        await context.SaveChangesAsync();
        return context;
    }

    private Mock<HttpMessageHandler> CreateHttpMessageHandler(string responseContent)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            });

        return handler;
    }

    [Fact]
    public async Task CalculateRevenue_ShouldReturnCorrectRevenue()
    {
        var context = await GetDbContext();

        var handler = CreateHttpMessageHandler("{\"rates\": {\"USD\": 0.25}}");
        var httpClient = new HttpClient(handler.Object);

        var service = new RevenueService(context, httpClient);

        var result = await service.CalculateRevenue("USD");

        Assert.Equal(75, result.TotalRevenue);
        Assert.Equal(175, result.TotalExpectedRevenue);
    }
}
