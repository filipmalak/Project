
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt.Services;
using System.Threading.Tasks;

namespace Projekt.Controller;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueService _revenueService;

        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentRevenue([FromQuery] string currency = "PLN")
        {
            var revenue = await _revenueService.CalculateRevenue(currency);
            return Ok(revenue);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductRevenue(int productId, [FromQuery] string currency = "PLN")
        {
            var revenue = await _revenueService.CalculateProductRevenue(productId, currency);
            return Ok(revenue);
        }
    }

