using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rocky_Models;

namespace Rocky_DataAccess.Repository.IRepository
{
    public interface IApplicationTypeRepository  : IRepository<ApplicationType>
    {
        void Update(ApplicationType obj);
    }
}