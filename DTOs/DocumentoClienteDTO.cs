using System.ComponentModel.DataAnnotations;

namespace HistoriaUsuario2.DTOs
{
    public class DocumentoClienteDTO
    {
        public string tipo { get; set; }
        public IFormFile archivo { get; set; }
        public int idSolicitudCredito { get; set; }
    }
}
