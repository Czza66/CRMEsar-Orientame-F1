using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class AtencionesRDA
    {
        [Key]
        public Guid AtencionID { get; set; }
        public int? IdAntiguo { get; set; }
        public int? CodigoPrestador {  get; set; }
        public string? tipoPrestador { get; set; }
        public string? NombrePrestador { get; set; }
        public string? profesion {get; set;}
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

        //Campos Foraneos
        [Required]
        public Guid PrestadorID { get; set; }
        public Prestadores? Prestador { get; set; }
    }
}
