using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess;
using Rocky_Models;
using Rocky_Models.ViewModels;
using Rocky_Utility;
using Rocky.Utility;
using Rocky_DataAccess.Repository.IRepository;

namespace Rocky.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IApplicationUserRepository _userRepo;
        private readonly IInquiryHeaderRepository _inqHRepo;
        private readonly IProductRepository _prodRepo;
        private readonly IInquiryDetailRepository _inqDRepo;


        
        public CartController(IWebHostEnvironment webHostEnvironment,IEmailSender emailSender,
        IApplicationUserRepository userRepo,IProductRepository prodRepo,
        IInquiryHeaderRepository inqHRepo,IInquiryDetailRepository inqDRepo)
        {
           
            _userRepo = userRepo;
            _prodRepo = prodRepo;
            _inqHRepo = inqHRepo;
            _inqDRepo = inqDRepo;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        
        }
        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        // GET: CartController
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Rocky_Utility.WC.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Rocky_Utility.WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(Rocky_Utility.WC.SessionCart);

            }
            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> prodList = _prodRepo.GetAll(u => prodInCart.Contains(u.Id));
            return View(prodList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]

        public IActionResult IndexPost()
        {

            return RedirectToAction(nameof(Summary));
        }

        public  IActionResult Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Rocky_Utility.WC.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Rocky_Utility.WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(Rocky_Utility.WC.SessionCart);

            }
            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> prodList = _prodRepo.GetAll(u => prodInCart.Contains(u.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _userRepo.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = prodList.ToList()
            };

        
            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task <IActionResult> SummaryPost(ProductUserVM ProductUserVM)
        {
            var claimsIdentity =(ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                +"templates" + Path.DirectorySeparatorChar.ToString()+
                "inquiry.html";

                var subject = "New Inquiry";
                string HtmlBody ="";

                using(StreamReader sr = System.IO.File.OpenText(PathToTemplate)){
                    HtmlBody = sr.ReadToEnd();
                }
                //Name:{0}
                //Email:{1}
                //Phone:{2}
                //Products:{3}
                StringBuilder productListSB = new StringBuilder();
                foreach(var prod in ProductUserVM.ProductList){
                    productListSB.Append($" - Name: {prod.Name} <span style ='font-size:14px;'> (ID: {prod.Id})</span><br/>");
                }
       

                string messageBody = string.Format(HtmlBody,
                    ProductUserVM.ApplicationUser.FullName,
                    ProductUserVM.ApplicationUser.Email,
                    ProductUserVM.ApplicationUser.PhoneNumber,
                  
                    productListSB.ToString()
                   
                );
                
            await _emailSender.SendEmailAsync(WC.EmailAdmin,subject,messageBody);
            InquiryHeader inquiryHeader = new InquiryHeader(){
                ApplicationUserId = claim.Value,
                FullName = ProductUserVM.ApplicationUser.FullName,
                Email = ProductUserVM.ApplicationUser.Email,
                PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
                InquiryDate = DateTime.Now
                
            };
            _inqHRepo.Add(inquiryHeader);
            _inqHRepo.Save();

            foreach(var prod in ProductUserVM.ProductList){
                InquiryDetail inquiryDetail = new InquiryDetail(){
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = prod.Id
                };
                _inqDRepo.Add(inquiryDetail);
                _inqDRepo.Save();
            }

            
            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation()
        {
            
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Rocky_Utility.WC.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(Rocky_Utility.WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(Rocky_Utility.WC.SessionCart);
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(Rocky_Utility.WC.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }

    }
}
