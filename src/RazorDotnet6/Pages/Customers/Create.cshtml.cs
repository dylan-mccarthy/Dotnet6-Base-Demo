using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDotnet6.Models;
using RazorDotnet6.Services;

namespace RazorDotnet6.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly CrmApiService _crmApiService;

        public CreateModel(CrmApiService crmApiService)
        {
            _crmApiService = crmApiService;
        }

        [BindProperty]
        public Customer Customer { get; set; } = new();

        public IActionResult OnGet()
        {
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
                var createdCustomer = await _crmApiService.CreateCustomerAsync(Customer);
                if (createdCustomer != null)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to create customer. Please try again.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the customer.");
                return Page();
            }
        }
    }
}