using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDotnet6.Data;
using APIDotnet6.Models;

namespace APIDotnet6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CrmDbContext _context;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(CrmDbContext context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Customers.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => 
                    c.FirstName.Contains(search) ||
                    c.LastName.Contains(search) ||
                    c.Email.Contains(search) ||
                    (c.Company != null && c.Company.Contains(search)));
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(c => c.Status == status);
            }

            // Apply pagination
            var totalCount = await query.CountAsync();
            var customers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.Contacts)
                .Include(c => c.Opportunities)
                .ToListAsync();

            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Page", page.ToString());
            Response.Headers.Add("X-Page-Size", pageSize.ToString());

            return Ok(customers);
        }

        // GET: api/customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Contacts)
                .Include(c => c.Opportunities)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if email already exists
            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
            {
                return BadRequest("A customer with this email already exists.");
            }

            customer.CreatedDate = DateTime.UtcNow;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        // PUT: api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Check if email already exists for another customer
            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email && c.Id != id))
            {
                return BadRequest("A customer with this email already exists.");
            }

            // Update properties
            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Company = customer.Company;
            existingCustomer.JobTitle = customer.JobTitle;
            existingCustomer.Address = customer.Address;
            existingCustomer.City = customer.City;
            existingCustomer.State = customer.State;
            existingCustomer.PostalCode = customer.PostalCode;
            existingCustomer.Country = customer.Country;
            existingCustomer.Status = customer.Status;
            existingCustomer.Notes = customer.Notes;
            existingCustomer.LastModifiedDate = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> CustomerExists(int id)
        {
            return await _context.Customers.AnyAsync(e => e.Id == id);
        }
    }
}