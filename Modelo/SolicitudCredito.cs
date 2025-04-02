using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoFinal.Modelo
{
    public class SolicitudCredito
    {
        [Key]
        public int idSolicitudCredito { get; set; }

        [Required(ErrorMessage = "La fecha de solicitud es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime fechaSolicitud { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(50, ErrorMessage = "El estado no puede exceder los 50 caracteres")]
        public string estado { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public double monto { get; set; }

        [Required(ErrorMessage = "Los ingresos son obligatorios")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Los ingresos deben ser mayores a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ingresos { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones exceden la longitud permitida")]
        public string observaciones { get; set; }

        [Required(ErrorMessage = "El id del cliente es obligatorio")]
        public int idCliente { get; set; }

        [Required(ErrorMessage = "El id del tipo de crédito es obligatorio")]
        public int idTipoCredito { get; set; }

        [Required(ErrorMessage = "El id del parámetro es obligatorio")]
        public int idParametro { get; set; }

        [ForeignKey("idCliente")]
        public virtual Cliente? Cliente { get; set; }

        [ForeignKey("idTipoCredito")]
        public virtual TipoCredito? TipoCredito { get; set; }

        [ForeignKey("idParametro")]
        public virtual Parametro? Parametro { get; set; }

        public virtual ICollection<Documento>? Documentos { get; set; }

        [JsonIgnore]
        public virtual ICollection<EvaluacionCredito>? Evaluaciones { get; set; }

        [JsonIgnore]
        public virtual CreditoAprobado? CreditoAprobado { get; set; }

        [JsonIgnore]
        public virtual ICollection<Notificacion>? Notificaciones { get; set; }
    }
}
