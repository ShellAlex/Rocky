using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Rocky_DataAccess;
using Rocky_Models;
using Rocky_Models.ViewModels;
using Rocky_Utility;
using Rocky.Utility;
using Rocky_DataAccess.Repository.IRepository;



namespace Rocky.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _prodRepo;
    private readonly ICategoryRepository _catRepo;

    public HomeController(ILogger<HomeController> logger,IProductRepository prodRepo,
    ICategoryRepository catRepo)
    {
        _logger = logger;
        _prodRepo = prodRepo;
        _catRepo = catRepo;
    }

    public IActionResult Index()
    {
        HomeVM homeVm = new HomeVM(){
            Products = _prodRepo.GetAll(includeProperties:"Category,ApplicationType"),
            Categories=_catRepo.GetAll()
        };
        return View(homeVm);
    }

    public IActionResult Details(int id){


 List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
        if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart)!=null
        &&HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count()>0){
            shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);

        }

        DetailsVM DetailsVM = new DetailsVM(){
                Product = _prodRepo.FirstOrDefault(u=>u.Id==id,includeProperties:"Category,ApplicationType"),
                ExistsInCart=false
        };

        foreach(var item in shoppingCartList){
            if(item.ProductId == id){
                DetailsVM.ExistsInCart =true;
            }
        }


        return View(DetailsVM);
    }

        [HttpPost,ActionName("Details")]
      public IActionResult DetailsPost(int id){
        List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
        if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart)!=null
        &&HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count()>0){
            shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
        }
        shoppingCartList.Add(new ShoppingCart{ProductId = id});
        HttpContext.Session.Set(WC.SessionCart,shoppingCartList);
        return RedirectToAction(nameof(Index));
    }
    
    
    
    public IActionResult RemoveFromCart(int id){
        List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
        if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart)!=null
        &&HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count()>0){
            shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
        }
        var itemToRemove = shoppingCartList.SingleOrDefault(u=>u.ProductId == id);
        if(itemToRemove!=null){
            shoppingCartList.Remove(itemToRemove);
        }

        HttpContext.Session.Set(WC.SessionCart,shoppingCartList);
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Privacy()
    {
        ViewBag.a = 200;
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
