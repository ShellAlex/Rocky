using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky_Models
{
    public class Math
    {
        [Key]
        public int Id { get; set; } 

        
        [Required]
        public string Text { get; set;}
    }
}