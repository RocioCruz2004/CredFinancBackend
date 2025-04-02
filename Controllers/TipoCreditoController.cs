using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyectoFinal.Data;
using proyectoFinal.Modelo;
using Microsoft.EntityFrameworkCore;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoCreditoController : ControllerBase
    {
        public readonly DBCconexion dbConexion;
        public TipoCreditoController(DBCconexion dbConexion)
        {
            this.dbConexion = dbConexion;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoCredito>>> Get()
        {
            var tipoCreditos = await dbConexion.TipoCredito.ToListAsync();
            return Ok(tipoCreditos);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TipoCredito tipoCredito)
        {
            if (tipoCredito == null)
            {
                return BadRequest("El objeto está vacío");
            }

            dbConexion.TipoCredito.Add(tipoCredito);
            await dbConexion.SaveChangesAsync();
            return Ok("Se insertó correctamente");
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] TipoCredito tipoCredito, int idTipoCredito)
        {
            if (idTipoCredito == null)
            {
                return BadRequest("El tipo credito esta vacio");
            }

            if (idTipoCredito == 0)
            {
                return BadRequest("El id de tipo de crédito está vacío");
            }

            var existeTipoCredito = await dbConexion.TipoCredito.FirstOrDefaultAsync(p => p.idTipoCredito == idTipoCredito);

            if (existeTipoCredito == null)
            {
                return NotFound("El id de tipo de credito no existe");
            }

            // Modificar los campos
            existeTipoCredito.nombre = tipoCredito.nombre;
            existeTipoCredito.descripcion = tipoCredito.descripcion;

            await dbConexion.SaveChangesAsync();
            return Ok("Se modificaron los datos correctamente");
        }
    }
}
