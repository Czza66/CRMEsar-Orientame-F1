using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones
{
    public class SliderPresentacionesCrearEditarVM
    {
        public string? sliderId {  get; set; }
        public string? ImagenRuta { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string? TextoAlternativo { get; set; }
        public string? UrlDestino { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; }
    }
}
