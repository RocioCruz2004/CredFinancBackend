namespace CooperativaFinanciera.DTOs
{
    public class EvaluacionCreditoDTO
    {
        public string decision { get; set; }
        public string comentarios { get; set; }
        public int idSolicitudCredito { get; set; }
        public int idAdministrador { get; set; }
    }
}