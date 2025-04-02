public class ContratoVisualizacionDTO
{
    public int Id { get; set; }
    public DateTime FechaGeneracion { get; set; }
    public DateTime? FechaFirma { get; set; }
    public string Contenido { get; set; }
    public string Estado { get; set; }
    public int IdSolicitudCredito { get; set; }
    public string NombreCliente { get; set; }
    public bool EstaFirmado { get; set; }
}