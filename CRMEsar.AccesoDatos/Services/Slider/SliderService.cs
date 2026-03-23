using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones;

namespace CRMEsar.AccesoDatos.Services.Slider
{
    public class SliderService : ISliderService
    {
        readonly IContenedorTrabajo _contenedorTrabajo;

        public SliderService(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        public List<SliderPresentacionesVM> ObtenerSlidersActivo() 
        {
            return _contenedorTrabajo.SliderPresentaciones
                .GetAll(S => S.Activo)
                .OrderBy(S => S.Orden)
                .Select(S => new SliderPresentacionesVM
                { 
                    ImagenRuta = S.ImagenRuta,
                    Titulo = S.Titulo,
                    Descripcion = S.Descripcion,
                    Orden = S.Orden,
                    Activo = S.Activo
                })
                .ToList();
        }
    }
}
