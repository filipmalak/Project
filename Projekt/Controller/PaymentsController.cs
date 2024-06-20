
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Context;
using Projekt.Models;

namespace Projekt.Controller;

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] Payment payment)
        {
            var contract = await _context.Contracts
                .Include(c => c.Payments)
                .FirstOrDefaultAsync(c => c.Id == payment.ContractId);

            if (contract == null)
            {
                return NotFound();
            }

            if (contract.EndDate < DateTime.UtcNow)
            {
                return BadRequest("The contract has expired.");
            }

            contract.Payments.Add(payment);
            await _context.SaveChangesAsync();

            if (contract.Payments.Sum(p => p.Amount) >= contract.DiscountedPrice)
            {
                contract.IsPaid = true;
                await _context.SaveChangesAsync();
            }

            return Ok(payment);
        }
    }

