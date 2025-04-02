namespace CooperativaFinanciera.DTOs
{
    public class NotificacionDTO
    {
        public string mensaje { get; set; }
        public string tipo { get; set; }
        public int idSolicitudCredito { get; set; }
        public int? idCreditoAprobado { get; set; }
    }
}