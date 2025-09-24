using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDotnet6.Data;
using APIDotnet6.Models;

namespace APIDotnet6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly CrmDbContext _context;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(CrmDbContext context, ILogger<ContactsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts(
            [FromQuery] int? customerId = null,
            [FromQuery] string? type = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Contacts.Include(c => c.Customer).AsQueryable();

            // Apply customer filter
            if (customerId.HasValue)
            {
                query = query.Where(c => c.CustomerId == customerId.Value);
            }

            // Apply type filter
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(c => c.Type == type);
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(c => c.Status == status);
            }

            // Apply pagination
            var totalCount = await query.CountAsync();
            var contacts = await query
                .OrderByDescending(c => c.ContactDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Page", page.ToString());
            Response.Headers.Add("X-Page-Size", pageSize.ToString());

            return Ok(contacts);
        }

        // GET: api/contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _context.Contacts
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // GET: api/contacts/customer/5
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContactsByCustomer(int customerId)
        {
            var contacts = await _context.Contacts
                .Where(c => c.CustomerId == customerId)
                .OrderByDescending(c => c.ContactDate)
                .ToListAsync();

            return Ok(contacts);
        }

        // POST: api/contacts
        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verify customer exists
            if (!await _context.Customers.AnyAsync(c => c.Id == contact.CustomerId))
            {
                return BadRequest("Customer not found.");
            }

            contact.CreatedDate = DateTime.UtcNow;
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            // Load the customer for the response
            await _context.Entry(contact)
                .Reference(c => c.Customer)
                .LoadAsync();

            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        // PUT: api/contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingContact = await _context.Contacts.FindAsync(id);
            if (existingContact == null)
            {
                return NotFound();
            }

            // Verify customer exists
            if (!await _context.Customers.AnyAsync(c => c.Id == contact.CustomerId))
            {
                return BadRequest("Customer not found.");
            }

            // Update properties
            existingContact.CustomerId = contact.CustomerId;
            existingContact.Type = contact.Type;
            existingContact.Subject = contact.Subject;
            existingContact.Description = contact.Description;
            existingContact.ContactDate = contact.ContactDate;
            existingContact.Status = contact.Status;
            existingContact.ContactedBy = contact.ContactedBy;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ContactExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ContactExists(int id)
        {
            return await _context.Contacts.AnyAsync(e => e.Id == id);
        }
    }
}