using System.ComponentModel.DataAnnotations;

namespace proyectoFinal.DTOs
{
    public class SolicitudCreditoClienteDTO
    {
        public double monto { get; set; }
        public decimal ingresos { get; set; }
        public string observaciones { get; set; }
        public int idTipoCredito { get; set; }
        public int idCliente { get; set; }
        public int idParametro { get; set; }
    }
}
