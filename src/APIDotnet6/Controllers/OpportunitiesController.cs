using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDotnet6.Data;
using APIDotnet6.Models;

namespace APIDotnet6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpportunitiesController : ControllerBase
    {
        private readonly CrmDbContext _context;
        private readonly ILogger<OpportunitiesController> _logger;

        public OpportunitiesController(CrmDbContext context, ILogger<OpportunitiesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/opportunities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Opportunity>>> GetOpportunities(
            [FromQuery] int? customerId = null,
            [FromQuery] string? stage = null,
            [FromQuery] string? status = null,
            [FromQuery] string? assignedTo = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Opportunities.Include(o => o.Customer).AsQueryable();

            // Apply customer filter
            if (customerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == customerId.Value);
            }

            // Apply stage filter
            if (!string.IsNullOrEmpty(stage))
            {
                query = query.Where(o => o.Stage == stage);
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status == status);
            }

            // Apply assigned to filter
            if (!string.IsNullOrEmpty(assignedTo))
            {
                query = query.Where(o => o.AssignedTo == assignedTo);
            }

            // Apply pagination
            var totalCount = await query.CountAsync();
            var opportunities = await query
                .OrderByDescending(o => o.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Page", page.ToString());
            Response.Headers.Add("X-Page-Size", pageSize.ToString());

            return Ok(opportunities);
        }

        // GET: api/opportunities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Opportunity>> GetOpportunity(int id)
        {
            var opportunity = await _context.Opportunities
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (opportunity == null)
            {
                return NotFound();
            }

            return Ok(opportunity);
        }

        // GET: api/opportunities/customer/5
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Opportunity>>> GetOpportunitiesByCustomer(int customerId)
        {
            var opportunities = await _context.Opportunities
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();

            return Ok(opportunities);
        }

        // GET: api/opportunities/stats
        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetOpportunityStats()
        {
            var totalOpportunities = await _context.Opportunities.CountAsync();
            var openOpportunities = await _context.Opportunities.CountAsync(o => o.Status == "Open");
            var wonOpportunities = await _context.Opportunities.CountAsync(o => o.Status == "Won");
            var lostOpportunities = await _context.Opportunities.CountAsync(o => o.Status == "Lost");
            
            var totalValue = await _context.Opportunities
                .Where(o => o.Status == "Open")
                .SumAsync(o => o.EstimatedValue);
            
            var wonValue = await _context.Opportunities
                .Where(o => o.Status == "Won")
                .SumAsync(o => o.EstimatedValue);

            var stageStats = await _context.Opportunities
                .Where(o => o.Status == "Open")
                .GroupBy(o => o.Stage)
                .Select(g => new { Stage = g.Key, Count = g.Count(), Value = g.Sum(o => o.EstimatedValue) })
                .ToListAsync();

            return Ok(new
            {
                TotalOpportunities = totalOpportunities,
                OpenOpportunities = openOpportunities,
                WonOpportunities = wonOpportunities,
                LostOpportunities = lostOpportunities,
                PipelineValue = totalValue,
                WonValue = wonValue,
                StageBreakdown = stageStats
            });
        }

        // POST: api/opportunities
        [HttpPost]
        public async Task<ActionResult<Opportunity>> CreateOpportunity(Opportunity opportunity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verify customer exists
            if (!await _context.Customers.AnyAsync(c => c.Id == opportunity.CustomerId))
            {
                return BadRequest("Customer not found.");
            }

            opportunity.CreatedDate = DateTime.UtcNow;
            _context.Opportunities.Add(opportunity);
            await _context.SaveChangesAsync();

            // Load the customer for the response
            await _context.Entry(opportunity)
                .Reference(o => o.Customer)
                .LoadAsync();

            return CreatedAtAction(nameof(GetOpportunity), new { id = opportunity.Id }, opportunity);
        }

        // PUT: api/opportunities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOpportunity(int id, Opportunity opportunity)
        {
            if (id != opportunity.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingOpportunity = await _context.Opportunities.FindAsync(id);
            if (existingOpportunity == null)
            {
                return NotFound();
            }

            // Verify customer exists
            if (!await _context.Customers.AnyAsync(c => c.Id == opportunity.CustomerId))
            {
                return BadRequest("Customer not found.");
            }

            // Update properties
            existingOpportunity.CustomerId = opportunity.CustomerId;
            existingOpportunity.Title = opportunity.Title;
            existingOpportunity.Description = opportunity.Description;
            existingOpportunity.EstimatedValue = opportunity.EstimatedValue;
            existingOpportunity.Probability = opportunity.Probability;
            existingOpportunity.Stage = opportunity.Stage;
            existingOpportunity.ExpectedCloseDate = opportunity.ExpectedCloseDate;
            existingOpportunity.AssignedTo = opportunity.AssignedTo;
            existingOpportunity.Status = opportunity.Status;
            existingOpportunity.Notes = opportunity.Notes;
            existingOpportunity.LastModifiedDate = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OpportunityExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/opportunities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOpportunity(int id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null)
            {
                return NotFound();
            }

            _context.Opportunities.Remove(opportunity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> OpportunityExists(int id)
        {
            return await _context.Opportunities.AnyAsync(e => e.Id == id);
        }
    }
}