using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Data;
using proyectoFinal.Modelo;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametroController : ControllerBase
    {
        private readonly DBCconexion dBConexion;

        public ParametroController(DBCconexion db)
        {
            dBConexion = db;
        }

        // Obtener todos los parámetros
        [HttpGet("Lista")]
        public async Task<ActionResult<List<Parametro>>> GetParametros()
        {
            var parametros = await dBConexion.Parametros.ToListAsync();
            if (parametros == null || !parametros.Any())
                return NotFound("No se encontraron parámetros configurados.");
            return Ok(parametros);
        }

        // Actualizar un parámetro existente
        [HttpPut("modificar")]
        public async Task<ActionResult> Update([FromBody] Parametro parametro, int idParametro)
        {
            if (idParametro == 0)
                return BadRequest("El id de parámetro está vacío");

            var existeParametro = await dBConexion.Parametros.FirstOrDefaultAsync(p => p.Id == idParametro);

            if (existeParametro == null)
                return NotFound("El parámetro no existe");

            existeParametro.TasaInteres = parametro.TasaInteres;
            existeParametro.PlazoMaximo = parametro.PlazoMaximo;
            existeParametro.FormatoContrato = parametro.FormatoContrato;

            await dBConexion.SaveChangesAsync();
            return Ok("Se modificaron los datos correctamente");
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> CrearParametro([FromBody] Parametro parametro)
        {
            // Validación para creación de parámetro
            if (parametro.TasaInteres <= 0 || parametro.PlazoMaximo <= 0)
                return BadRequest("La tasa de interés y el plazo máximo deben ser valores positivos.");

            // Asignar el IdAdministrador como 1 (predefinido) hasta que el sistema de login esté activo
            if (parametro.IdAdministrador == 0)  // Si no se pasa un IdAdministrador
            {
                parametro.IdAdministrador = 1;  // Predefinir el Id del administrador
            }

            // Verificar que el administrador exista (si fuera necesario en el futuro)
            var idadministrador = await dBConexion.Administrador.FindAsync(parametro.IdAdministrador);
            if (idadministrador == null)
            {
                return BadRequest("El id del administrador no existe");
            }

            // Agregar el nuevo parámetro
            dBConexion.Parametros.Add(parametro);
            await dBConexion.SaveChangesAsync();
            return Ok("Parámetro agregado correctamente.");
        }


        // Eliminar un parámetro
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarParametro(int id)
        {
            var parametro = await dBConexion.Parametros.FindAsync(id);
            if (parametro == null)
                return NotFound("Parámetro no encontrado");

            dBConexion.Parametros.Remove(parametro);
            await dBConexion.SaveChangesAsync();

            return Ok("Parámetro eliminado correctamente.");
        }
    }
}
