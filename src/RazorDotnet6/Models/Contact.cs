using System.ComponentModel.DataAnnotations;

namespace RazorDotnet6.Models
{
    public class Contact
    {
        public int Id { get; set; }
        
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Subject { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Display(Name = "Contact Date")]
        public DateTime ContactDate { get; set; }
        
        public string Status { get; set; } = "Completed";
        
        [Display(Name = "Contacted By")]
        [StringLength(100)]
        public string? ContactedBy { get; set; }
        
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        
        // Navigation property
        public Customer? Customer { get; set; }
    }
}