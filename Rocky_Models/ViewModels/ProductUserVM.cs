using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rocky_Models;

namespace Rocky_Models.ViewModels;

public class ProductUserVM
{
    public ProductUserVM()
    {
        ProductList = new List<Product>();
        
    }
    public ApplicationUser ApplicationUser { get; set; }
    public IList<Product> ProductList { get; set; }
}
