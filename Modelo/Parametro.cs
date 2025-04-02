using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoFinal.Modelo
{
    public class Parametro
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La tasa de interés es obligatoria")]
        [Range(0, 100, ErrorMessage = "La tasa de interés debe estar entre 0 y 100")]
        public double TasaInteres { get; set; }

        [Required(ErrorMessage = "El plazo es requerido")]
        public int PlazoMaximo { get; set; }


        [Required(ErrorMessage = "El formato de contrato es obligatorio")]
        [StringLength(100, ErrorMessage = "El formato de contrato no puede exceder los 100 caracteres")]
        public string FormatoContrato { get; set; }

        public int IdAdministrador { get; set; }
        [ForeignKey(nameof(IdAdministrador))]
        public Administrador? administrador { get; set; }
    }
}
