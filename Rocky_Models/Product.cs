using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Rocky_Models
{
    public class Product
    {
        [Key]
        [AllowNull]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [ValidateNever]
        public string? ShortDesc { get; set; }

        public string Description { get; set; }
        [Range(1,int.MaxValue)]
        public double Price { get; set; }

        [ValidateNever]
        public string Image { get; set; }

        [ValidateNever]
        [Display(Name ="Category Type")]
        public int CategoryId { get; set; }
        
        [ValidateNever]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        
        [ValidateNever]
        [Display(Name = "Application Type")]
        public int ApplicationTypeId { get; set; }
        
        [ValidateNever]
        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }
    }
}