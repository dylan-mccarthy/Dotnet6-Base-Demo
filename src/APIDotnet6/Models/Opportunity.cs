using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIDotnet6.Models
{
    public class Opportunity
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedValue { get; set; }
        
        [Range(0, 100)]
        public int Probability { get; set; } // Percentage (0-100)
        
        [Required]
        [StringLength(50)]
        public string Stage { get; set; } = "Prospecting"; // Prospecting, Qualification, Proposal, Negotiation, Closed Won, Closed Lost
        
        public DateTime ExpectedCloseDate { get; set; }
        
        [StringLength(100)]
        public string? AssignedTo { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastModifiedDate { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "Open"; // Open, Won, Lost
        
        public string? Notes { get; set; }
        
        // Navigation property
        public virtual Customer Customer { get; set; } = null!;
    }
}