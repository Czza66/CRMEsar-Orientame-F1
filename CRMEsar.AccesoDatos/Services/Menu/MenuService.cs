using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.MenuVM;

namespace CRMEsar.AccesoDatos.Services.Menu
{
    public class MenuService : IMenuService
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public MenuService(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        public async Task<List<ModuloConSeccionesVM>> ObtenerMenuJerarquicoPorRolAsync(Guid rolId) 
        {
            var permisos = await _contenedorTrabajo.PermisosModulosSecciones.GetAllAsync(
                p => p.RolId == rolId && (!p.Temporal || p.FechaInicio <= DateTime.Now && p.FechaFin >= DateTime.Now)
                );

            var seccionIds = permisos.Select(p => p.SeccionId).Distinct().ToList();

            var Secciones = await _contenedorTrabajo.ModulosSecciones.GetAllAsync(
                s => seccionIds.Contains(s.seccionId) && s.visible,
                includeProperties: "Estado"
                );

            var moduloIds = Secciones.Select(s => s.ModuloId).Distinct().ToList();
            var modulos = await _contenedorTrabajo.Modulo.GetAllAsync(
                m => moduloIds.Contains(m.moduloId) && m.visible);

            var resultado = modulos
                .OrderBy(m => m.orden)
                .Select(modulo => new ModuloConSeccionesVM
                {
                    NombreModulo = modulo.nombre,
                    Icono = modulo.icono,
                    Secciones = Secciones
                        .Where(s => s.ModuloId == modulo.moduloId &&
                        s.Estado.Nombre != "Eliminado" &&
                        s.Estado.Nombre != "Inactivo")
                        .OrderBy(s => s.orden)
                        .Select(s => new menuVM
                        {
                            Nombre = s.nombre,
                            Area = s.area,
                            Controller = s.controller,
                            Action = s.action,
                            Orden = s.orden
                        }).ToList()
                }).ToList();

            return resultado;
        }
    }
}
