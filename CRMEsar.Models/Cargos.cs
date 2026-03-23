using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class Cargos
    {
        [Key]
        public Guid CargoId { get; set; }

        public int IdAntiguo { get; set; }
        public string Cargo {  get; set; } 
        public string Abreviatura { get; set; }

        public Guid EstadoId { get; set; }

        [ForeignKey(nameof(EstadoId))]
        public Estados Estado { get; set; }
    }
}
