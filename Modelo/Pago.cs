using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoFinal.Modelo
{
    public enum EstadoPago
    {
        Pendiente = 0,
        Pagado = 1,
        Atrasado = 2
    }

    public class Pago
    {
        [Key]
        public int Id { get; set; }

        // No es necesario que el cliente ingrese el ID, se asigna automáticamente
        [ForeignKey(nameof(ClienteId))]
        public Cliente Cliente { get; set; }

        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La fecha de pago es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Por favor ingrese una fecha válida.")]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "El monto del pago es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser un valor positivo.")]
        public double Monto { get; set; }

        [Required(ErrorMessage = "El estado del pago es obligatorio.")]
        public EstadoPago Estado { get; set; }

        public void RegistrarPago() { /* Lógica para registrar el pago */ }
    }
}
