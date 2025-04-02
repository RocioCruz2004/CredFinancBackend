using System;

namespace proyectoFinal.DTOs
{
    public class AlertaDTO
    {
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public int SolicitudId { get; set; }
        public int? PagoId { get; set; }
    }
}