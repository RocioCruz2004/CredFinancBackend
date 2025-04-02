using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoFinal.Modelo
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo idUsuario es obligatorio")]
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "El género es requerido")]
        [StringLength(20, ErrorMessage = "El género no puede tener más de 20 caracteres")]
        public string genero { get; set; }

        [Required(ErrorMessage = "La nacionalidad es requerida")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "La nacionalidad debe tener entre 3 y 50 caracteres")]
        public string nacionalidad { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "La dirección debe tener entre 10 y 100 caracteres")]
        public string direccion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(15, ErrorMessage = "El teléfono no puede tener más de 15 caracteres.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Por favor ingrese una fecha válida.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La fecha de registro es requerida")]
        [DataType(DataType.Date)]
        public DateTime fechaRegistro { get; set; }

        [ForeignKey("idUsuario")]
        public virtual Usuario? Usuario { get; set; }
        [JsonIgnore]

        public ICollection<ReporteFinanciero>? ReportesFinancieros { get; set; }
        public ICollection<Pago>? Pagos { get; set; }
        public ICollection<CreditoAprobado>? CreditosAprobados { get; set; }
        public virtual ICollection<SolicitudCredito>? SolicitudesCredito { get; set; }


    }
}
