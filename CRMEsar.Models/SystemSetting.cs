using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class SystemSetting
    {
        [Key]
        public Guid SettingId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Key { get; set; } = null!;
        // Ej: "Security.2FA.RepromptValue"

        [MaxLength(80)]
        public string? Group { get; set; }
        // Ej: "Security"

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(30)]
        public string DataType { get; set; } = null!;
        // "string", "int", "decimal", "bool", "datetime", "json"

        // 🔹 Valores tipados (solo uno debe usarse según DataType)
        public string? ValueString { get; set; }

        public int? ValueInt { get; set; }

        public decimal? ValueDecimal { get; set; }

        public bool? ValueBit { get; set; }

        public DateTime? ValueDateTimeUtc { get; set; }

        public string? ValueJson { get; set; }

        // 🔹 Control
        public bool IsActive { get; set; } = true;

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        [MaxLength(256)]
        public string? UpdatedBy { get; set; }
    }
}
