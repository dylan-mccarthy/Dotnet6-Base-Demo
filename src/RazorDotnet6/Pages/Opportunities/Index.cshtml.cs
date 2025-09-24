using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDotnet6.Models;
using RazorDotnet6.Services;

namespace RazorDotnet6.Pages.Opportunities
{
    public class IndexModel : PageModel
    {
        private readonly CrmApiService _crmApiService;

        public IndexModel(CrmApiService crmApiService)
        {
            _crmApiService = crmApiService;
        }

        public List<Opportunity> Opportunities { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                Opportunities = await _crmApiService.GetOpportunitiesAsync(pageSize: 50);
            }
            catch (Exception)
            {
                Opportunities = new List<Opportunity>();
            }
        }
    }
}