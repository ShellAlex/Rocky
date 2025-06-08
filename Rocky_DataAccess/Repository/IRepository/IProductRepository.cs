using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky_Models;

namespace Rocky_DataAccess.Repository.IRepository
{
    public interface IProductRepository  : IRepository<Product>
    {
        void Update(Product obj);

        IEnumerable<SelectListItem>GetAllDropdownList(string obj);
       
    }
}