using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.Models.CategoryModels;
using MyApp.MyAppDataAccessLayer;
using MyAppDataAccessLayer.Infrestructure.IRepository;

namespace MyWebApp.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.categories = _unitOfWork.Category.GetAll(); ;
            return View(categoryVM);
        }
        //[HttpGet]
        //public IActionResult Create()
        //{
        //   return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Category category)
        //{
        //    if (ModelState.IsValid) { 
        //    _unitOfWork.Category.Add(category);
        //    _unitOfWork.Save();
        //        TempData["success"] = "Category Created Done";
        //    return RedirectToAction("Index");
        //    }
        //    return View(category);
        //}
        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            CategoryVM vM = new CategoryVM();
            if (id == null || id == 0)
            {
                return View(vM);
            }
            else
            {
                vM.Category = _unitOfWork.Category.GetT(x => x.Id == id);
                if (vM.Category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(vM);
                }
                
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(CategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                if(vm.Category.Id == 0)
                {
                    _unitOfWork.Category.Add(vm.Category);
                    TempData["success"] = "Category Created Done";
                }
                else
                {
                    _unitOfWork.Category.Update(vm.Category);
                    TempData["success"] = "Category Updated Done";
                }
                
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Done";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteData(int? id)
        {
            var category = _unitOfWork.Category.GetT(x => x.Id == id);
            if (category == null) {
                return NotFound();
            }
            _unitOfWork.Category.Delete(category);
            _unitOfWork.Save();
            TempData["success"] = "Category Removed Done";
            return RedirectToAction("Index");
        }


    }
}
