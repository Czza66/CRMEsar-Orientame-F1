using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones
{
    public class SliderPresentacionesVM
    {
        public string ImagenRuta { get; set; }
        public string Titulo {  get; set; }
        public string Descripcion { get; set; }
        public int Orden {  get; set; }
        public bool Activo { get; set; }

        public string SliderPresentacionEncriptado { get; set; }
    }
}
