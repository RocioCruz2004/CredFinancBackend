using System;

namespace proyectoFinal.DTOs
{
    public class PagoDTO
    {
        public int Id { get; set; }
        public DateTime FechaPago { get; set; }
        public double Monto { get; set; }
        public string Estado { get; set; }
        public int? SolicitudId { get; set; }
    }

    public class PagoDetalleDTO : PagoDTO
    {
        public string ClienteNombre { get; set; }
        public double MontoCredito { get; set; }
        public double TasaAplicada { get; set; }
        public int PlazoAprobado { get; set; }
    }
}