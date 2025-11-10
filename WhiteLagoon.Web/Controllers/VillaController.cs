using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers;

public class VillaController : Controller
{
    private readonly IVillaRepository  _repository; 

    public VillaController(IVillaRepository repository)
    {
        _repository = repository;
    }
    public IActionResult Index()
    {
        var villas = _repository.GetAll();  
        return View(villas);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Villa obj)
    {
        if (obj.Name == obj.Description)
        {
            ModelState.AddModelError("","The description cannot exactly match the name");
        }
        if (ModelState.IsValid)
        {
            _repository.Add(obj);
            _repository.Save();
            TempData["success"] = "Villa created successfully";
            return RedirectToAction("Index","Villa");
        }
        TempData["error"] = "Villa could not be created";
        return View(obj);
    }
    public IActionResult Update(int villaId)
    {
        // linq expression
        Villa? obj = _repository.Get(u=>u.Id == villaId);
        if (obj == null)
            return  RedirectToAction("Error","Home");
        
        return View(obj);
    }

    [HttpPost]
    public IActionResult Update(Villa obj)
    {
        if (ModelState.IsValid && obj.Id > 0)
        {
            _repository.Update(obj);
            _repository.Save();
            TempData["success"] = "Villa updated successfully";
            return RedirectToAction("Index","Villa");
        }
        TempData["error"] = "Villa could not be updated";
        return View();   
    }
    public IActionResult Delete(int villaId)
    {
        // linq expression 
        Villa? obj = _repository.Get(u=>u.Id == villaId);
        if (obj == null)
            return  RedirectToAction("Error","Home");
        
        return View(obj);
    }

    [HttpPost]
    public IActionResult Delete(Villa obj)
    {
        Villa? objFromDb = _repository.Get(u=>u.Id == obj.Id); 
        if (objFromDb is not null)
        {
            _repository.Remove(objFromDb);
            _repository.Save();
            TempData["success"] = "Villa deleted successfully";
            return RedirectToAction("Index","Villa");
        }
        TempData["error"] = "Villa could not deleted";
        return View();   
    }
}