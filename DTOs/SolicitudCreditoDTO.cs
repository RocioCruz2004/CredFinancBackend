using System;
using System.Collections.Generic;

namespace CooperativaFinanciera.DTOs
{
    public class SolicitudCreditoListDTO
    {
        public int idSolicitudCredito { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public string estado { get; set; }
        public double monto { get; set; }
        public string nombreCliente { get; set; }
        public string tipoCredito { get; set; }
    }

    public class SolicitudCreditoDetailDTO
    {
        public int idSolicitudCredito { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public string estado { get; set; }
        public double monto { get; set; }
        public decimal ingresos { get; set; }
        public string observaciones { get; set; }
        public ClienteDTO cliente { get; set; }
        public TipoCreditoDTO tipoCredito { get; set; }
        public ParametroDTO parametro { get; set; }
        public List<DocumentoDTO> documentos { get; set; }
    }

    public class ClienteDTO
    {
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public DateTime? fechaNacimiento { get; set; }
        public string genero { get; set; }
        public string nacionalidad { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
    }

    public class TipoCreditoDTO
    {
        public int idTipoCredito { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }

    public class ParametroDTO
    {
        public int idParametro { get; set; }
        public double tasaInteres { get; set; }
        public int plazo { get; set; }
        public string formatoContrato { get; set; }
    }

    public class DocumentoDTO
    {
        public int idDocumento { get; set; }
        public string tipo { get; set; }
        public DateTime fechaCarga { get; set; }
        public string urlArchivo { get; set; }
    }

    public class SolicitudStatusDTO
    {
        public string estado { get; set; }
    }
}