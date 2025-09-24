using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDotnet6.Models;
using RazorDotnet6.Services;

namespace RazorDotnet6.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly CrmApiService _crmApiService;

        public EditModel(CrmApiService crmApiService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _crmApiService.UpdateCustomerAsync(Customer);
                if (result)
                {
                    return RedirectToPage("./Details", new { id = Customer.Id });
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update customer. Please try again.");
                    return Page();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the customer.");
                return Page();
            }
        }
    }
}