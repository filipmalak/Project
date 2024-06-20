
using Microsoft.AspNetCore.Mvc;
using Projekt.Context;
using Projekt.Models;

namespace Projekt.Controller;

    [Route("api/[controller]")]
    [ApiController]
    public class SoftwareController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SoftwareController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddSoftware([FromBody] Software software)
        {
            _context.Softwares.Add(software);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSoftwareById), new { id = software.Id }, software);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSoftware(int id, [FromBody] Software updatedSoftware)
        {
            var software = await _context.Softwares.FindAsync(id);
            if (software == null)
            {
                return NotFound();
            }

            software.Name = updatedSoftware.Name;
            software.Description = updatedSoftware.Description;
            software.Version = updatedSoftware.Version;
            software.Category = updatedSoftware.Category;
            software.IsSubscriptionAvailable = updatedSoftware.IsSubscriptionAvailable;
            software.IsSinglePurchaseAvailable = updatedSoftware.IsSinglePurchaseAvailable;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSoftware(int id)
        {
            var software = await _context.Softwares.FindAsync(id);
            if (software == null)
            {
                return NotFound();
            }

            _context.Softwares.Remove(software);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSoftwareById(int id)
        {
            var software = await _context.Softwares.FindAsync(id);
            if (software == null)
            {
                return NotFound();
            }

            return Ok(software);
        }
    }

