using System;

namespace proyectoFinal.DTOs
{
    public class FirmaContratoDTO
    {
        public int idContrato { get; set; }
        public string codigoVerificacion { get; set; }
    }

    public class RespuestaFirmaDTO
    {
        public bool exito { get; set; }
        public string mensaje { get; set; }
        public string estado { get; set; }
        public DateTime? fechaFirma { get; set; }
    }

    public class SolicitudDobleFactor
    {
        public int idContrato { get; set; }
        public string metodo { get; set; }
    }

    public class RespuestaDobleFactor
    {
        public bool exito { get; set; }
        public string mensaje { get; set; }
        public string codigoVerificacion { get; set; }
    }
}

