using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDotnet6.Models;
using RazorDotnet6.Services;

namespace RazorDotnet6.Pages.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly CrmApiService _crmApiService;

        public DetailsModel(CrmApiService crmApiService)
        {
            _crmApiService = crmApiService;
        }

        public Customer? Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Customer = await _crmApiService.GetCustomerAsync(id.Value);
                if (Customer == null)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }

            return Page();
        }
    }
}