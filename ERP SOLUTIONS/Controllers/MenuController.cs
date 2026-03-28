using ERP_SOLUTIONS.Models.ViewModels;
using ERP_SOLUTIONS.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace ERP_SOLUTIONS.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuService _menuService;

        public MenuController(MenuService menuService) {
            _menuService = menuService;
        }

        // GET
        public IActionResult CreateMenu()
        {
            MenuPageVM all = _menuService.GetAllMenus();
            return View(all);
        }

        // POST
        [HttpPost]
        public IActionResult CreateMenu(MenuSectionVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            string message = _menuService.CreateMenu(vm);

            return RedirectToAction("CreateMenu");
        }
    }

}
