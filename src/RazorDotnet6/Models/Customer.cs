using System.ComponentModel.DataAnnotations;

namespace RazorDotnet6.Models
{
    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(255)]
        public string? Company { get; set; }
        
        [Display(Name = "Job Title")]
        [StringLength(100)]
        public string? JobTitle { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [StringLength(100)]
        public string? City { get; set; }
        
        [StringLength(20)]
        public string? State { get; set; }
        
        [Display(Name = "Postal Code")]
        [StringLength(20)]
        public string? PostalCode { get; set; }
        
        [StringLength(100)]
        public string? Country { get; set; }
        
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        
        [Display(Name = "Last Modified")]
        public DateTime? LastModifiedDate { get; set; }
        
        public string Status { get; set; } = "Active";
        
        public string? Notes { get; set; }
        
        // Navigation properties
        public List<Contact> Contacts { get; set; } = new();
        public List<Opportunity> Opportunities { get; set; } = new();
        
        // Computed properties
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}