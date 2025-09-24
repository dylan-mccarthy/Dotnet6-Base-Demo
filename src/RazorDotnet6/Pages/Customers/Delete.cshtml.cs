using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDotnet6.Models;
using RazorDotnet6.Services;

namespace RazorDotnet6.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        private readonly CrmApiService _crmApiService;

        public DeleteModel(CrmApiService crmApiService)
        {
            _crmApiService = crmApiService;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _crmApiService.GetCustomerAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }
            Customer = customer;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var result = await _crmApiService.DeleteCustomerAsync(id.Value);
                if (result)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete customer. Please try again.");
                    return Page();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while deleting the customer.");
                return Page();
            }
        }
    }
}