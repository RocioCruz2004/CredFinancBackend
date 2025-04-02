using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoFinal.Modelo
{
    public class AuditLog
    {
        [Key]
        public int idAuditLog { get; set; }

        [Required(ErrorMessage = "El campo acción es obligatorio")]
        [StringLength(200, ErrorMessage = "La acción no cumple con los valores permitidos")]
        public string accion { get; set; }

        [Required(ErrorMessage = "El campo usuario es obligatorio")]
        [StringLength(100, ErrorMessage = "El usuario no cumple con los valores permitidos")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "El campo fecha es obligatorio")]
        public DateTime fecha { get; set; }

        [StringLength(500, ErrorMessage = "Los detalles exceden la longitud permitida")]
        public string detalles { get; set; }

        [Required(ErrorMessage = "El idAdministrador es obligatorio")]
        public int idAdministrador { get; set; }

        [ForeignKey("idAdministrador")]
        [JsonIgnore]
        public virtual Administrador Administrador { get; set; }
    }
}
