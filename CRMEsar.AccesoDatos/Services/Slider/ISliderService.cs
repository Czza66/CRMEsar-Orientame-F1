using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones;

namespace CRMEsar.AccesoDatos.Services.Slider
{
    public interface ISliderService
    {
        List<SliderPresentacionesVM> ObtenerSlidersActivo();
    }
}
