using System.ComponentModel.DataAnnotations;

namespace RazorDotnet6.Models
{
    public class Opportunity
    {
        public int Id { get; set; }
        
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Display(Name = "Estimated Value")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal EstimatedValue { get; set; }
        
        [Range(0, 100)]
        [Display(Name = "Probability (%)")]
        public int Probability { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Stage { get; set; } = "Prospecting";
        
        [Display(Name = "Expected Close Date")]
        [DataType(DataType.Date)]
        public DateTime ExpectedCloseDate { get; set; }
        
        [Display(Name = "Assigned To")]
        [StringLength(100)]
        public string? AssignedTo { get; set; }
        
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        
        [Display(Name = "Last Modified")]
        public DateTime? LastModifiedDate { get; set; }
        
        public string Status { get; set; } = "Open";
        
        public string? Notes { get; set; }
        
        // Navigation property
        public Customer? Customer { get; set; }
    }
}