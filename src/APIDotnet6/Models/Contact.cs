using System.ComponentModel.DataAnnotations;

namespace APIDotnet6.Models
{
    public class Contact
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty; // Phone, Email, Meeting, etc.
        
        [Required]
        [StringLength(100)]
        public string Subject { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public DateTime ContactDate { get; set; } = DateTime.UtcNow;
        
        [StringLength(20)]
        public string Status { get; set; } = "Completed"; // Scheduled, Completed, Cancelled
        
        [StringLength(100)]
        public string? ContactedBy { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public virtual Customer Customer { get; set; } = null!;
    }
}