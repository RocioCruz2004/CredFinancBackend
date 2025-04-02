using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace proyectoFinal.Modelo
{
    public class EvaluacionCredito
    {
        [Key]
        public int idEvaluacionCredito { get; set; }

        [Required(ErrorMessage = "El campo decisión es obligatorio")]
        [StringLength(20, ErrorMessage = "La decisión no cumple con los valores permitidos")]
        public string decision { get; set; }

        [StringLength(500, ErrorMessage = "Los comentarios exceden la longitud permitida")]
        public string comentarios { get; set; }

        [Required(ErrorMessage = "La fecha de evaluación es obligatoria")]
        public DateTime fechaEvaluacion { get; set; }

        [Required(ErrorMessage = "El id de solicitud de crédito es obligatorio")]
        public int idSolicitudCredito { get; set; }

        [Required(ErrorMessage = "El id de administrador es obligatorio")]
        public int idAdministrador { get; set; }

        [ForeignKey("idSolicitudCredito")]
        [JsonIgnore]
        public virtual SolicitudCredito SolicitudCredito { get; set; }

        [ForeignKey("idAdministrador")]
        [JsonIgnore]
        public virtual Administrador Administrador { get; set; }
    }
}
