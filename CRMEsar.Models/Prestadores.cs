 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class Prestadores
    {
        [Key]
        public Guid PrestadorId { get; set; }
        public int IdAntiguo { get; set; }
        public string Profesion { get; set; }
        public int Codigo { get; set; }
        public string? RecomendadoPor { get; set; }
        public string? Telefono { get; set; }

        [StringLength(70)]
        public string? Celular { get; set; }
        public bool Orientador { get; set; }
        public bool PermiteILVE { get; set; }
        public string? TipoServicio { get; set; } 
        public string TipoPrestador { get; set; }

        //Campos Foraneos
        [ForeignKey("EstadoId")]
        public Guid? EstadoId { get; set; }
        public Estados? Estado { get; set; }

        [Required]
        public Guid UserID { get; set; }
        public ApplicationUser? User { get; set; }

    }
}
