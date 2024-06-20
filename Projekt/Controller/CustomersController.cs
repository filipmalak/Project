using Projekt.Models;

using Microsoft.AspNetCore.Mvc;
using Projekt.Context;


namespace Projekt.Controller;

    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            if (customer is IndividualCustomer individual)
            {
                _context.IndividualCustomers.Add(individual);
            }
            else if (customer is CompanyCustomer company)
            {
                _context.CompanyCustomers.Add(company);
            }
            else
            {
                return BadRequest("Invalid customer type");
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null || customer.IsDeleted)
            {
                return NotFound();
            }

            if (customer is IndividualCustomer individual && updatedCustomer is IndividualCustomer updatedIndividual)
            {
                individual.FirstName = updatedIndividual.FirstName;
                individual.LastName = updatedIndividual.LastName;
                individual.Address = updatedIndividual.Address;
                individual.Email = updatedIndividual.Email;
                individual.PhoneNumber = updatedIndividual.PhoneNumber;
            }
            else if (customer is CompanyCustomer company && updatedCustomer is CompanyCustomer updatedCompany)
            {
                company.CompanyName = updatedCompany.CompanyName;
                company.Address = updatedCompany.Address;
                company.Email = updatedCompany.Email;
                company.PhoneNumber = updatedCompany.PhoneNumber;
            }
            else
            {
                return BadRequest("Invalid customer type");
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            if (customer is CompanyCustomer)
            {
                return BadRequest("Cannot delete a company");
            }

            customer.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null || customer.IsDeleted)
            {
                return NotFound();
            }

            return Ok(customer);
        }
    }


