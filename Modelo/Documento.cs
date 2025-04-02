using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace proyectoFinal.Modelo
{
    public class Documento
    {
        [Key]
        public int idDocumento { get; set; }

        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [StringLength(50, ErrorMessage = "El tipo no cumple con los valores permitidos")]
        public string tipo { get; set; }

        [Required(ErrorMessage = "La fecha de carga es obligatoria")]
        public DateTime fechaCarga { get; set; }

        [Required(ErrorMessage = "La URL del archivo es obligatoria")]
        [StringLength(255, ErrorMessage = "La URL excede la longitud permitida")]
        public string urlArchivo { get; set; }

        [Required(ErrorMessage = "El id de solicitud de crédito es obligatorio")]
        public int idSolicitudCredito { get; set; }

        [ForeignKey("idSolicitudCredito")]
        [JsonIgnore]
        public virtual SolicitudCredito SolicitudCredito { get; set; }
    }
}
