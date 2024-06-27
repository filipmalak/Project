
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Context;
using Projekt.Models;

namespace Projekt.Controller;

    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContractsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract([FromBody] Contract contract)
        {
            var existingContract = await _context.Contracts
                .AnyAsync(c => c.CustomerId == contract.CustomerId && c.SoftwareId == contract.SoftwareId && !c.IsPaid);
            if (existingContract)
            {
                return BadRequest("Klient już ma aktywny kontrakt");
            }

            var highestDiscount = contract.Software.Discounts
                .Where(d => d.StartDate <= DateTime.UtcNow && d.EndDate >= DateTime.UtcNow)
                .OrderByDescending(d => d.Percentage)
                .FirstOrDefault();

            if (highestDiscount != null)
            {
                contract.DiscountedPrice = contract.BasePrice - (contract.BasePrice * (decimal)highestDiscount.Percentage / 100);
            }
            else
            {
                contract.DiscountedPrice = contract.BasePrice;
            }

            var hasPreviousContract = await _context.Contracts
                .AnyAsync(c => c.CustomerId == contract.CustomerId && c.IsPaid);
            if (hasPreviousContract)
            {
                contract.DiscountedPrice -= contract.DiscountedPrice * 0.05m;  // 5% znizki
            }

            contract.EndDate = contract.StartDate.AddDays(30);

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContractById), new { id = contract.Id }, contract);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContractById(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Payments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return Ok(contract);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null || contract.IsPaid)
            {
                return NotFound();
            }

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

