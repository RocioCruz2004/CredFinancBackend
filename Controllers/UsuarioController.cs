using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyectoFinal.Data;
using proyectoFinal.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly DBCconexion dbConexion;
        private static Dictionary<string, int> intentosFallidos = new();

        public UsuarioController(DBCconexion dbConexion)
        {
            this.dbConexion = dbConexion;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await dbConexion.Usuario
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u => u.email == usuario.email);

            if (user == null)
            {
                return Unauthorized("Usuario no encontrado.");
            }

            if (intentosFallidos.ContainsKey(user.email))
            {
                if (intentosFallidos[user.email] >= 5)
                {
                    return Unauthorized(new
                    {
                        mensaje = "Cuenta bloqueada por múltiples intentos fallidos.",
                        restaurar = "Para restablecer la contraseña, use la ruta /api/usuario/reset-password"
                    });
                }
            }

            if (usuario.password != user.password)
            {
                if (!intentosFallidos.ContainsKey(user.email))
                {
                    intentosFallidos[user.email] = 1;
                }
                else
                {
                    intentosFallidos[user.email]++;
                }

                return Unauthorized(new
                {
                    mensaje = "Credenciales incorrectas.",
                    intentosRestantes = 5 - intentosFallidos[user.email]
                });
            }

            if (intentosFallidos.ContainsKey(user.email))
            {
                intentosFallidos[user.email] = 0;
            }

            return Ok(new
            {
                token = $"simpletoken-{user.idUsuario}-{DateTime.Now.Ticks}",
                usuario = new
                {
                    idUsuario = user.idUsuario,
                    nombre = user.nombre,
                    email = user.email,
                    idCliente = user.Cliente?.Id
                },
                mensaje = "Inicio de sesión exitoso.",
                tieneCliente = user.Cliente != null
            });
        }

        [HttpPost("retablecer-contraseña")]
        public async Task<IActionResult> UpdatePassword([FromBody] Usuario usuario)
        {
            var user = await dbConexion.Usuario.FirstOrDefaultAsync(u => u.email == usuario.email);
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            if (string.IsNullOrWhiteSpace(usuario.password))
            {
                return BadRequest("La nueva contraseña no puede estar vacía.");
            }

            user.password = usuario.password;
            dbConexion.Usuario.Update(user);
            await dbConexion.SaveChangesAsync();

            return Ok("Contraseña actualizada correctamente.");
        }


        [HttpGet("Lista Usuarios")]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            var usuario = await dbConexion.Usuario.ToListAsync();
            return Ok(usuario);
        }

        [HttpPost("crear-cuenta-usuario")]
        public async Task<ActionResult<dynamic>> Post([FromBody] Usuario usuario)
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

            if (await dbConexion.Usuario.AnyAsync(u => u.email == usuario.email))
            {
                return Conflict(new
                {
                    success = false,
                    message = "El email ya está registrado"
                });
            }

            try
            {
                dbConexion.Usuario.Add(usuario);
                await dbConexion.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    idUsuario = usuario.idUsuario,
                    nombre = usuario.nombre,
                    email = usuario.email,
                    message = "Usuario creado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message
                });
            }
        }

        [HttpPut("Editar cuenta usuario")]
        public async Task<ActionResult> Update([FromBody] Usuario usuario, int idusuario)
        {
            if (idusuario == null)
            {
                return BadRequest("Usuario vacio");
            }

            if (idusuario == 0)
            {
                return BadRequest("El id de usuario está vacío");
            }

            var existeUsuario = await dbConexion.Usuario.FirstOrDefaultAsync(p => p.idUsuario == idusuario);

            if (existeUsuario == null)
            {
                return NotFound("el id usuario no existe");
            }

            // Modificar los campos
            existeUsuario.nombre = usuario.nombre;
            existeUsuario.email = usuario.email;
            existeUsuario.password = usuario.password;

            await dbConexion.SaveChangesAsync();
            return Ok("Se modificaron los datos correctamente");
        }
    }
}