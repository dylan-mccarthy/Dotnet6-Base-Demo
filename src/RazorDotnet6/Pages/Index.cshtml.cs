using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDotnet6.Models;
using RazorDotnet6.Services;

namespace RazorDotnet6.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly CrmApiService _crmApiService;

        public IndexModel(ILogger<IndexModel> logger, CrmApiService crmApiService)
        {
            _logger = logger;
            _crmApiService = crmApiService;
        }

        public int CustomerCount { get; set; }
        public int OpenOpportunityCount { get; set; }
        public int RecentContactCount { get; set; }
        public decimal PipelineValue { get; set; }
        public List<Customer>? RecentCustomers { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // Get recent customers (limited to 5 for dashboard)
                var customers = await _crmApiService.GetCustomersAsync(pageSize: 5);
                RecentCustomers = customers;
                CustomerCount = customers.Count; // This is just the recent count, would need a separate endpoint for total count

                // Get opportunities to calculate stats
                var opportunities = await _crmApiService.GetOpportunitiesAsync(pageSize: 100); // Get more for stats
                OpenOpportunityCount = opportunities.Count(o => o.Status == "Open");
                PipelineValue = opportunities.Where(o => o.Status == "Open").Sum(o => o.EstimatedValue);

                // Get recent contacts (last 7 days)
                var contacts = await _crmApiService.GetContactsAsync(pageSize: 50);
                var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
                RecentContactCount = contacts.Count(c => c.ContactDate >= sevenDaysAgo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                // Set default values
                CustomerCount = 0;
                OpenOpportunityCount = 0;
                RecentContactCount = 0;
                PipelineValue = 0;
                RecentCustomers = new List<Customer>();
            }
        }
    }
}
