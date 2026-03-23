using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMEsar.AccesoDatos.Services.Menu
{
    public class MenuViewComponent :ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public MenuViewComponent(IMenuService menuService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _menuService = menuService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var rolNombre = roles.FirstOrDefault();

            var rol = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == rolNombre);
            var rolId = rol.Id;

            var menu = await _menuService.ObtenerMenuJerarquicoPorRolAsync(rolId);

            return View(menu);
        }
    }
}
