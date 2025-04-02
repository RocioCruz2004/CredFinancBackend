using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoFinal.Modelo
{
    public class Administrador
    {
        [Key]
        public int idAdministrador { get; set; }

        [Required(ErrorMessage = "El campo idUsuario es obligatorio")]
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El rol debe tener entre 3 y 50 caracteres")]
        public string rol { get; set; }

        [Required(ErrorMessage = "El campo cargo es obligatorio")]
        [StringLength(50, ErrorMessage = "El cargo no cumple con los valores permitidos")]
        public string cargo { get; set; }

        [Required(ErrorMessage = "El campo departamento es obligatorio")]
        [StringLength(50, ErrorMessage = "El departamento no cumple con los valores permitidos")]
        public string departamento { get; set; }

        [Required(ErrorMessage = "El nivel de acceso es requerido")]
        [Range(1, 10, ErrorMessage = "El nivel de acceso debe estar entre 1 y 10")]
        public int nivelAcceso { get; set; }

        [ForeignKey("idUsuario")]
        public virtual Usuario? Usuario { get; set; }

        [JsonIgnore]
        public virtual ICollection<EvaluacionCredito>? Evaluaciones { get; set; }

        [JsonIgnore]
        public virtual ICollection<AuditLog>? AuditLogs { get; set; }
    }
}
