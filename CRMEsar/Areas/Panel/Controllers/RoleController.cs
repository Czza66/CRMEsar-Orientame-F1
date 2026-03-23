using CRMEsar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                ModelState.AddModelError("", "El nombre es obligatorio");
                return View();
            }

            if (await _roleManager.RoleExistsAsync(nombre))
            {
                ModelState.AddModelError("", "El rol ya existe");
                return View();
            }

            var resultado = await _roleManager.CreateAsync(new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = nombre
            });

            if (resultado.Succeeded)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al crear el rol");
            return View();
        }
    }

}