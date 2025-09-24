using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorDotnet6.Models;
using RazorDotnet6.Services;

namespace RazorDotnet6.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly CrmApiService _crmApiService;

        public IndexModel(CrmApiService crmApiService)
        {
            _crmApiService = crmApiService;
        }

        public List<Customer> Customers { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                Customers = await _crmApiService.GetCustomersAsync(SearchTerm, StatusFilter, pageSize: 50);
            }
            catch (Exception)
            {
                // Handle error - maybe show a message to user
                Customers = new List<Customer>();
            }
        }
    }
}