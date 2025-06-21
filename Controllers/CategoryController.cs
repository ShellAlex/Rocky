using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess;
using Rocky_Models;
using Rocky_Utility;
using Rocky.Utility;
using Rocky_DataAccess.Repository.IRepository;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
     public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catRepo;
        
        public CategoryController(ICategoryRepository catRepo)
        {
            _catRepo = catRepo;
        }
        
        public IActionResult Index(){
            IEnumerable<Category> objList = _catRepo.GetAll();
            return View(objList);
        }

        //Get - Create
        public IActionResult Create(){

            return View();
        }
        
        //POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj){
            if(ModelState.IsValid){
                _catRepo.Add(obj);
                _catRepo.Save();
                TempData[WC.Success]="Category created succesfully";
                return RedirectToAction("Index");
            }
            TempData[WC.Error]="Error while creating category";
            return View(obj);
        }


        //GEt Edit
        public IActionResult Edit(int? id){
            if(id==null | id==0){
                return NotFound();
            }
            var obj =_catRepo.Find(id.GetValueOrDefault());
            if(obj==null){
                return NotFound();
            }
            return View(obj);
        }

        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj){
            if(ModelState.IsValid){
                _catRepo.Update(obj);
                _catRepo.Save();
                TempData[WC.Success]="Category edited succesfully";
                return RedirectToAction("Index");
            }
            TempData[WC.Error]="Error while deleting";
            return View(obj);
        }

        //GEt DELETE
        public IActionResult Delete(int? id){
            if(id==null | id==0){
                return NotFound();
            }
            var obj =_catRepo.Find(id.GetValueOrDefault());
            if(obj==null){
                return NotFound();
            }
            TempData[WC.Success]="Category deleted succesfully";
            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id){
            var obj = _catRepo.Find(id.GetValueOrDefault());
            if(obj==null){
                return NotFound();
            }
            _catRepo.Remove(obj);
            _catRepo.Save();
            return RedirectToAction("Index");
        }

    }
}