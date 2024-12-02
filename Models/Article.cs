using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        
        public string Title { get; set; }
        [Required]
        public string Text { get; set;}
    }
}