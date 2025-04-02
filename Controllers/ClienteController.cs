using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyectoFinal.Data;
using proyectoFinal.Modelo;
using Microsoft.EntityFrameworkCore;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public readonly DBCconexion dbConexion;
        public ClienteController(DBCconexion dbConexion)
        {
            this.dbConexion = dbConexion;
        }

        [HttpGet("Enlistar Clientes")]
        public async Task<ActionResult<List<Cliente>>> Get()
        {
            var clientes = await dbConexion.Clientes.Include(c => c.Usuario)
                                   .ToListAsync();
            return Ok(clientes);
        }

        [HttpPost("crear-cliente")]
        public async Task<ActionResult<dynamic>> Post([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Datos inválidos",
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            try
            {
                /*// Verificar que el usuario exista
                var usuarioExistente = await dbConexion.Usuario
                    .AnyAsync(u => u.idUsuario == cliente.idUsuario);

                if (!usuarioExistente)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Usuario no encontrado"
                    });
                }*/

                cliente.fechaRegistro = DateTime.Now;
                dbConexion.Clientes.Add(cliente);
                await dbConexion.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    idCliente = cliente.Id,
                    message = "Cliente registrado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al registrar cliente",
                    error = ex.Message
                });
            }
        }

        [HttpPut("Modificar datos cliente")]
        public async Task<ActionResult> Update([FromBody] Cliente cliente, int idCliente)
        {
            if (idCliente == null)
            {
                return BadRequest("El cliente esta vacio");
            }

            if (idCliente == 0)
            {
                return BadRequest("El id de cliente está vacío");
            }

            var existeCliente = await dbConexion.Clientes.FirstOrDefaultAsync(c => c.Id == idCliente);

            if (existeCliente == null)
            {
                return NotFound("El id cliente no existe");
            }

            // Modificar los campos
            existeCliente.FechaNacimiento = cliente.FechaNacimiento;
            existeCliente.genero = cliente.genero;
            existeCliente.nacionalidad = cliente.nacionalidad;
            existeCliente.Telefono = cliente.Telefono;
            existeCliente.direccion = cliente.direccion;
            existeCliente.fechaRegistro = cliente.fechaRegistro;
            existeCliente.idUsuario = cliente.idUsuario;

            await dbConexion.SaveChangesAsync();
            return Ok("Se modificaron los datos correctamente");
        }
    }
}