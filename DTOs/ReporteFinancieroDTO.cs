using System;
using System.Collections.Generic;

namespace proyectoFinal.DTOs
{
    public class ReporteFinancieroDTO
    {
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public double TotalPagado { get; set; }
        public double TotalMorosidad { get; set; }
        public int PagosPendientes { get; set; }
        public int PagosAtrasados { get; set; }
        public int SolicitudesActivas { get; set; }
        public List<PagoDTO> HistorialPagos { get; set; }
    }
}