using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Prestadores
{
    public class PrestadoresEditarVM
    {
        public string? PrestadorId { get; set; }

        [Display(Name = "Profesión")]
        public string Profesion { get; set; }

        [Display(Name = "Código")]
        public int Codigo { get; set; }
        public string? RecomendadoPor { get; set; }
        [Display(Name ="Teléfono")]
        public string Telefono { get; set; }

        [StringLength(70)]
        public string? Celular { get; set; }

        public bool Orientador { get; set; }
        public bool PermiteILVE { get; set; }
        public string? TipoServicio { get; set; }
        public string? TipoPrestador { get; set; }
        public IEnumerable<SelectListItem>? ListaTipoPrestador { get; set; }

        //Campos info del usuario
        public string? NombreCompleto { get; set; }
        public string? Correo { get; set; }

        [Display(Name = "País")]
        public string? Pais { get; set; }
        public string? Ciudad { get; set; }

        //Campos Foraneos

        public Guid? EstadoId { get; set; }
        public IEnumerable<SelectListItem>? ListaEstados { get; set; }
    }
}
