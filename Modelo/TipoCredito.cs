using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace proyectoFinal.Modelo
{
    public class TipoCredito
    {
        [Key]
        public int idTipoCredito { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no cumple con los valores permitidos")]
        public string nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción excede la longitud permitida")]
        public string descripcion { get; set; }
    }
}
