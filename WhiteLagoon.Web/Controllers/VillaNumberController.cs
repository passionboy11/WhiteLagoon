using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public VillaNumberController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
        return View(villaNumbers);
    }

    public IActionResult Create()
    {
        VillaNumberVM villaNumberVM = new()
        {
            VillaList = _unitOfWork.Villa.GetAll().Select(u=>new SelectListItem 
            {
                Text = u.Name,
                Value = u.Id.ToString()
            })
        };
        //ViewData  
        // IEnumerable<SelectListItem> list =_context.Villas.ToList().Select(u=>new SelectListItem 
        // {
        //     Text = u.Name,
        //     Value = u.Id.ToString()
        // // });
        // ViewData["VillaList"] = list;
        return View(villaNumberVM);
    }

    [HttpPost]
    public IActionResult Create(VillaNumberVM obj)
    {
        bool roomNumberExists = _unitOfWork.VillaNumber.Any(u=>u.Villa_Number == obj.VillaNumber.Villa_Number);
        
        // ModelState.Remove("Villa");
        if (ModelState.IsValid && !roomNumberExists)
        {
            _unitOfWork.VillaNumber.Add(obj.VillaNumber);
            _unitOfWork.Save();
            TempData["success"] = "Villa created successfully";
            return RedirectToAction("Index","VillaNumber");
        }

        if (roomNumberExists)
        {
            TempData["error"] = "The room already exists";
        }
        // repopulate dropdown on failure
        obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString()
        });
        
        return View(obj);
    }
    public IActionResult Update(int villaNumberId)
    {
        // linq expression
        VillaNumberVM villaNumberVM = new()
        {
            VillaList = _unitOfWork.Villa.GetAll().Select(u=>new SelectListItem 
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            VillaNumber = _unitOfWork.VillaNumber.Get(u=>u.Villa_Number == villaNumberId)
        };
       
        if (villaNumberVM.VillaNumber == null)
            return  RedirectToAction("Error","Home");
        
        return View(villaNumberVM);
    }

    [HttpPost]
    public IActionResult Update(VillaNumberVM villaNumberVM)
    {
        // ModelState.Remove("Villa");
        if (ModelState.IsValid)
        {
            _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
            _unitOfWork.Save();
            TempData["success"] = "Villa has been updated successfully";
            return RedirectToAction("Index","VillaNumber");
        }
        
        // repopulate dropdown on failure
        villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString()
        });
        
        return View(villaNumberVM);
    }
    public IActionResult Delete(int villaNumberId)
    {
        // linq expression
        VillaNumberVM villaNumberVM = new()
        {
            VillaList = _unitOfWork.Villa.GetAll().Select(u=>new SelectListItem 
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            VillaNumber = _unitOfWork.VillaNumber.Get(u=>u.Villa_Number == villaNumberId)
        };
       
        if (villaNumberVM.VillaNumber == null)
            return  RedirectToAction("Error","Home");
        
        return View(villaNumberVM);
    }

    [HttpPost]
    public IActionResult Delete(VillaNumberVM villaNumberVM)
    {
        VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get
            (u=>u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number); 
        if (objFromDb is not null)
        {
            _unitOfWork.VillaNumber.Remove(objFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Villa number deleted successfully";
            return RedirectToAction("Index","VillaNumber");
        }
        TempData["error"] = "Villa could not deleted";
        return View();   
    }
}