using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153505_Shevtsova_D.Models;
namespace Web_153505_Shevtsova_D.Controllers;

public class Home : Controller
{
    // GET
    public IActionResult Index()
    {
        
        ViewData["text"] = "Лабораторная работа 2";
        return View(new SelectList(new List<ListDemo>
        {
            new ListDemo { Id = 1, Name = "Item 1" },
            new ListDemo { Id = 2, Name = "Item 2" },
            new ListDemo { Id = 3, Name = "Item 3" },
            new ListDemo { Id = 4, Name = "Item 4" }
        }, "Id", "Name"));
    }
}
    