using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.AtencionesRda
{
    public class AteencionesRdaTablaVM
    {
        public int? CodigoPrestador { get; set; }
        public string? TipoPrestador { get; set; }
        public string? NombrePrestador { get; set; }
        public string? Profesion { get; set; }
        public string? Pais { get; set; }
        public string? Departamento { get; set; }
        public string? Ciudad { get; set; }
        public string? Donante { get; set; }
        public string? Proyecto { get; set; }
        public DateOnly? FechaCarga { get; set; }
        public DateOnly? FechaAtencion { get; set; }
        public string? IdentidadDisociada { get; set; }
        public int? Edad { get; set; }
        public string? GrupoEtareo { get; set; }
        public string? TipoConsulta { get; set; }
        public string? MetodoAnticonceptivo { get; set; }
        public DateOnly? FechaAnticonceptivo { get; set; }
        public bool? MacLargaDuracion { get; set; }
        public bool? MacModerno { get; set; }
        public string? MedioRemision { get; set; }
        public string? MetodoAnticonceptivoMac { get; set; }
        public DateOnly? FechaAnticonceptivoMac { get; set; }
        public string? Utero { get; set; }
        public int? Weeks { get; set; }
        public bool? ILVE { get; set; }
        public bool? Telemedicina { get; set; }
    }
}
