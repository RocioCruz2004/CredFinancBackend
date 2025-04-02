using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoFinal.Modelo
{
    public class Contrato
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "La fecha de generación es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime fechaGeneracion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? fechaFirma { get; set; }

        [Required(ErrorMessage = "El contenido del contrato es obligatorio")]
        [Column(TypeName = "nvarchar(max)")]
        public string contenido { get; set; }

        [Required(ErrorMessage = "El estado del contrato es obligatorio")]
        [StringLength(50, ErrorMessage = "El estado no puede exceder los 50 caracteres")]
        public string estado { get; set; } // GENERADO, PENDIENTE_FIRMA, FIRMADO, RECHAZADO

        [StringLength(255)]
        public string? firmaDigital { get; set; }

        [StringLength(100)]
        public string? metodoAutenticacion { get; set; }

        [StringLength(255)]
        public string? codigoVerificacion { get; set; }

        [Required(ErrorMessage = "El id de solicitud de crédito es obligatorio")]
        public int idSolicitudCredito { get; set; }

        [ForeignKey("idSolicitudCredito")]
        [JsonIgnore]
        public virtual SolicitudCredito SolicitudCredito { get; set; }
    }
}