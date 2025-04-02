using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace proyectoFinal.Modelo
{
    public class ReporteFinanciero
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del cliente es obligatorio.")]
        public int ClienteId { get; set; }

        [ForeignKey(nameof(ClienteId))]
        [JsonIgnore]  // Evitar la serialización recursiva de Cliente
        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = "La fecha de generación del reporte es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Por favor ingrese una fecha válida.")]
        public DateTime FechaGeneracion { get; set; }

        // Aquí ya no es necesario un mensaje de error, ya que estos se calcularán
        public double TotalPagado { get; set; }
        public double TotalMorosidad { get; set; }

        public void GenerarReporte() { /* Lógica para generar el reporte, con cálculos de total pagado y morosidad */ }
    }
}
