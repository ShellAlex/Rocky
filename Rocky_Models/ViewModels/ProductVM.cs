using System.Net.Http.Headers;
using System.Collections.Generic;
//using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Rocky_Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem> ApplicationTypeSelectList { get; set; }

    }
}