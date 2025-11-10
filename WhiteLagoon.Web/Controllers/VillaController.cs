using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers;

public class VillaController : Controller
{
    private readonly ApplicationDbContext _context;

    public VillaController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var villas = _context.Villas.ToList();  
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
            _context.Villas.Add(obj);
            _context.SaveChanges();
            TempData["success"] = "Villa created successfully";
            return RedirectToAction("Index","Villa");
        }
        TempData["error"] = "Villa could not be created";
        return View(obj);
    }
    public IActionResult Update(int villaId)
    {
        // linq expression
        Villa? obj = _context.Villas.FirstOrDefault(u=>u.Id == villaId);
        if (obj == null)
            return  RedirectToAction("Error","Home");
        
        return View(obj);
    }

    [HttpPost]
    public IActionResult Update(Villa obj)
    {
        if (ModelState.IsValid && obj.Id > 0)
        {
            _context.Villas.Update(obj);
            _context.SaveChanges();
            TempData["success"] = "Villa updated successfully";
            return RedirectToAction("Index","Villa");
        }
        TempData["error"] = "Villa could not be updated";
        return View();   
    }
    public IActionResult Delete(int villaId)
    {
        // linq expression 
        Villa? obj = _context.Villas.FirstOrDefault(u=>u.Id == villaId);
        if (obj == null)
            return  RedirectToAction("Error","Home");
        
        return View(obj);
    }

    [HttpPost]
    public IActionResult Delete(Villa obj)
    {
        Villa? objFromDb = _context.Villas.FirstOrDefault(u=>u.Id == obj.Id); 
        if (objFromDb is not null)
        {
            _context.Villas.Remove(objFromDb);
            _context.SaveChanges();
            TempData["success"] = "Villa deleted successfully";
            return RedirectToAction("Index","Villa");
        }
        TempData["error"] = "Villa could not deleted";
        return View();   
    }
}