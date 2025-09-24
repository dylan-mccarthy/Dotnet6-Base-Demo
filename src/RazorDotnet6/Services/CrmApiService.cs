using RazorDotnet6.Models;
using System.Text.Json;
using System.Text;

namespace RazorDotnet6.Services
{
    public class CrmApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public CrmApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        // Customer methods
        public async Task<List<Customer>> GetCustomersAsync(string? search = null, string? status = null, int page = 1, int pageSize = 10)
        {
            var queryString = new List<string>();
            if (!string.IsNullOrEmpty(search)) queryString.Add($"search={Uri.EscapeDataString(search)}");
            if (!string.IsNullOrEmpty(status)) queryString.Add($"status={Uri.EscapeDataString(status)}");
            queryString.Add($"page={page}");
            queryString.Add($"pageSize={pageSize}");

            var url = $"customers?{string.Join("&", queryString)}";
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Customer>>(json, _jsonOptions) ?? new List<Customer>();
            }
            
            return new List<Customer>();
        }

        public async Task<Customer?> GetCustomerAsync(int id)
        {
            var response = await _httpClient.GetAsync($"customers/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Customer>(json, _jsonOptions);
            }
            
            return null;
        }

        public async Task<Customer?> CreateCustomerAsync(Customer customer)
        {
            var json = JsonSerializer.Serialize(customer, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("customers", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Customer>(responseJson, _jsonOptions);
            }
            
            return null;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            var json = JsonSerializer.Serialize(customer, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"customers/{customer.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"customers/{id}");
            return response.IsSuccessStatusCode;
        }

        // Contact methods
        public async Task<List<Contact>> GetContactsAsync(int? customerId = null, string? type = null, string? status = null, int page = 1, int pageSize = 10)
        {
            var queryString = new List<string>();
            if (customerId.HasValue) queryString.Add($"customerId={customerId}");
            if (!string.IsNullOrEmpty(type)) queryString.Add($"type={Uri.EscapeDataString(type)}");
            if (!string.IsNullOrEmpty(status)) queryString.Add($"status={Uri.EscapeDataString(status)}");
            queryString.Add($"page={page}");
            queryString.Add($"pageSize={pageSize}");

            var url = $"contacts?{string.Join("&", queryString)}";
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Contact>>(json, _jsonOptions) ?? new List<Contact>();
            }
            
            return new List<Contact>();
        }

        public async Task<Contact?> GetContactAsync(int id)
        {
            var response = await _httpClient.GetAsync($"contacts/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Contact>(json, _jsonOptions);
            }
            
            return null;
        }

        public async Task<Contact?> CreateContactAsync(Contact contact)
        {
            var json = JsonSerializer.Serialize(contact, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("contacts", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Contact>(responseJson, _jsonOptions);
            }
            
            return null;
        }

        public async Task<bool> UpdateContactAsync(Contact contact)
        {
            var json = JsonSerializer.Serialize(contact, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"contacts/{contact.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"contacts/{id}");
            return response.IsSuccessStatusCode;
        }

        // Opportunity methods
        public async Task<List<Opportunity>> GetOpportunitiesAsync(int? customerId = null, string? stage = null, string? status = null, string? assignedTo = null, int page = 1, int pageSize = 10)
        {
            var queryString = new List<string>();
            if (customerId.HasValue) queryString.Add($"customerId={customerId}");
            if (!string.IsNullOrEmpty(stage)) queryString.Add($"stage={Uri.EscapeDataString(stage)}");
            if (!string.IsNullOrEmpty(status)) queryString.Add($"status={Uri.EscapeDataString(status)}");
            if (!string.IsNullOrEmpty(assignedTo)) queryString.Add($"assignedTo={Uri.EscapeDataString(assignedTo)}");
            queryString.Add($"page={page}");
            queryString.Add($"pageSize={pageSize}");

            var url = $"opportunities?{string.Join("&", queryString)}";
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Opportunity>>(json, _jsonOptions) ?? new List<Opportunity>();
            }
            
            return new List<Opportunity>();
        }

        public async Task<Opportunity?> GetOpportunityAsync(int id)
        {
            var response = await _httpClient.GetAsync($"opportunities/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Opportunity>(json, _jsonOptions);
            }
            
            return null;
        }

        public async Task<Opportunity?> CreateOpportunityAsync(Opportunity opportunity)
        {
            var json = JsonSerializer.Serialize(opportunity, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("opportunities", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Opportunity>(responseJson, _jsonOptions);
            }
            
            return null;
        }

        public async Task<bool> UpdateOpportunityAsync(Opportunity opportunity)
        {
            var json = JsonSerializer.Serialize(opportunity, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"opportunities/{opportunity.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteOpportunityAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"opportunities/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<object?> GetOpportunityStatsAsync()
        {
            var response = await _httpClient.GetAsync("opportunities/stats");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(json, _jsonOptions);
            }
            
            return null;
        }
    }
}