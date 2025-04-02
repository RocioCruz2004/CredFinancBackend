using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoFinal.Modelo
{
    public class CreditoAprobado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de aprobación es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Por favor ingrese una fecha válida.")]
        public DateTime FechaAprobacion { get; set; }

        [Required(ErrorMessage = "El monto aprobado es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto aprobado debe ser un valor positivo.")]
        public double MontoAprobado { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La tasa aplicada debe ser un valor positivo.")]
        public double TasaAplicada { get; set; }

        [Required(ErrorMessage = "El plazo aprobado es obligatorio")]
        public int plazoAprobado { get; set; }

        [Required(ErrorMessage = "El id de solicitud de crédito es obligatorio")]
        public int idSolicitudCredito { get; set; }

        [ForeignKey("idSolicitudCredito")]
        [JsonIgnore]
        public virtual SolicitudCredito SolicitudCredito { get; set; }

        [JsonIgnore]
        public virtual ICollection<Notificacion> Notificaciones { get; set; }
    }
}
