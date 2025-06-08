using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using Rocky_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rocky_DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Rocky_DataAccess.Repository
{
    public class InquiryDetailRepository : Repository<InquiryDetail>, IInquiryDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public  InquiryDetailRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }


       

        public void Update(InquiryDetail obj)
        {
            _db.InquiryDetail.Update(obj);
        }
    }
}