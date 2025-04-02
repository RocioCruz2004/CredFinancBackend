using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace proyectoFinal.Modelo
{
    public class Notificacion
    {
        [Key]
        public int idNotificacion { get; set; }

        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [StringLength(500, ErrorMessage = "El mensaje no cumple con los valores permitidos")]
        public string mensaje { get; set; }

        [Required(ErrorMessage = "La fecha de envío es obligatoria")]
        public DateTime fechaEnvio { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        [StringLength(20, ErrorMessage = "El tipo no cumple con los valores permitidos")]
        public string tipo { get; set; }

        [Required(ErrorMessage = "El id de solicitud de crédito es obligatorio")]
        public int idSolicitudCredito { get; set; }

        public int? idCreditoAprobado { get; set; }

        [ForeignKey("idSolicitudCredito")]
        [JsonIgnore]
        public virtual SolicitudCredito SolicitudCredito { get; set; }

        [ForeignKey("idCreditoAprobado")]
        [JsonIgnore]
        public virtual CreditoAprobado CreditoAprobado { get; set; }
    }
}
