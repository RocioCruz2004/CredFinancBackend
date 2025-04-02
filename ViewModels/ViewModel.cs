namespace proyectoFinal.ViewModels
{
    public class ViewModel
    {
        public class SolicitudCreditoViewModel
        {
            public DateTime FechaSolicitud { get; set; }
            public string Estado { get; set; }
            public double Monto { get; set; }
            public decimal Ingresos { get; set; }
            public string Observaciones { get; set; }
            public TipoCreditoViewModel TipoCredito { get; set; }
            public List<DocumentoViewModel> Documentos { get; set; }
        }

        public class TipoCreditoViewModel
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
        }

        public class DocumentoViewModel
        {
            public string Tipo { get; set; }
            public string ArchivoUrl { get; set; }
            public string FechaCarga { get; set; }
        }
    }
}
