
using Microsoft.AspNetCore.Mvc;
using Projekt.Context;
using Projekt.Models;

namespace Projekt.Controller;

    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiscountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscount([FromBody] Discount discount)
        {
            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDiscountById), new { id = discount.Id }, discount);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount(int id, [FromBody] Discount updatedDiscount)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            discount.Name = updatedDiscount.Name;
            discount.Description = updatedDiscount.Description;
            discount.Percentage = updatedDiscount.Percentage;
            discount.StartDate = updatedDiscount.StartDate;
            discount.EndDate = updatedDiscount.EndDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscountById(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            return Ok(discount);
        }
    }

