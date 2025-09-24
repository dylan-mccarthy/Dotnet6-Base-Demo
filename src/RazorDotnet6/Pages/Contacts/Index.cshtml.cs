using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDotnet6.Models;
using RazorDotnet6.Services;

namespace RazorDotnet6.Pages.Contacts
{
    public class IndexModel : PageModel
    {
        private readonly CrmApiService _crmApiService;

        public IndexModel(CrmApiService crmApiService)
        {
            _crmApiService = crmApiService;
        }

        public List<Contact> Contacts { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                Contacts = await _crmApiService.GetContactsAsync(pageSize: 50);
            }
            catch (Exception)
            {
                Contacts = new List<Contact>();
            }
        }
    }
}