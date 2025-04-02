using System;

namespace proyectoFinal.DTOs
{
    // DTO para obtener información del contrato
    public class ContratoDTO
    {
        public int id { get; set; }
        public DateTime fechaGeneracion { get; set; }
        public DateTime? fechaFirma { get; set; }
        public string contenido { get; set; }
        public string estado { get; set; }
        public string firmaDigital { get; set; }
        public int idSolicitudCredito { get; set; }
        public string nombreCliente { get; set; }
        public double montoAprobado { get; set; }
        public double tasaInteres { get; set; }
        public int plazo { get; set; }
    }

    // DTO para crear un nuevo contrato
    public class ContratoCreateDTO
    {
        public string contenido { get; set; }
        public int idSolicitudCredito { get; set; }
    }


   
}