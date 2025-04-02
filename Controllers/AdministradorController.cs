using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyectoFinal.Data;
using proyectoFinal.Modelo;
using Microsoft.EntityFrameworkCore;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradorController : ControllerBase
    {
        public readonly DBCconexion dBConexion;
        private static Dictionary<string, int> intentosFallidos = new();

        public AdministradorController(DBCconexion dbConexion)
        {
            this.dBConexion = dbConexion;
        }
        [HttpGet("Ver")]
        public async Task<ActionResult<List<Administrador>>> Get()
        {
            var administradores = await dBConexion.Administrador.Include(a => a.Usuario).ToListAsync();
            return Ok(administradores);
        }

        [HttpPost("Crear")]
        public async Task<ActionResult> Post([FromBody] Administrador administrador)
        {
            if (administrador == null)
            {
                return BadRequest("El objeto está vacío");
            }

            dBConexion.Administrador.Add(administrador);
            await dBConexion.SaveChangesAsync();
            return Ok("Se insertó correctamente");
        }
        [HttpPut("Editar perfil adm")]
        public async Task<ActionResult> Update([FromBody] Administrador administrador, int idAdministrador)
        {
            if (idAdministrador == null)
            {
                return BadRequest("El administrador esta vacio");
            }

            if (idAdministrador == 0)
            {
                return BadRequest("El id de administrador está vacío");
            }

            var existeAdministrador = await dBConexion.Administrador.FirstOrDefaultAsync(a => a.idAdministrador == idAdministrador);

            if (existeAdministrador == null)
            {
                return NotFound("El id de administrador no existe");
            }

            // Modificar los campos
            existeAdministrador.rol = administrador.rol;
            existeAdministrador.cargo = administrador.cargo;
            existeAdministrador.departamento = administrador.departamento;
            existeAdministrador.nivelAcceso = administrador.nivelAcceso;
            existeAdministrador.idUsuario = administrador.idUsuario;

            await dBConexion.SaveChangesAsync();
            return Ok("Se modificaron los datos correctamente");
        }
        [HttpPost("Registro")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] Cliente cliente)
        {
            /*if (await dbConexion.Usuarios.AnyAsync(u => u.email == usuario.email))
            {
                return BadRequest("El correo ya está registrado.");
            }*/

            dBConexion.Clientes.Add(cliente);
            await dBConexion.SaveChangesAsync();

            return Ok("Usuario registrado exitosamente.");
        }
        /*public readonly DbConexion dbConexion;
        private static Dictionary<string, int> intentosFallidos = new();

        public AdministradorController(DbConexion dbConexion)
        {
            this.dbConexion = dbConexion;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> RegistrarAdministrador([FromBody] Administrador administrador)
        {
            dbConexion.Administrador.Add(administrador);
            await dbConexion.SaveChangesAsync();

            return Ok("Administrador registrado exitosamente.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Administrador administrador)
        {
            var admin = await dbConexion.Administrador.FirstOrDefaultAsync(a => a.rol == administrador.rol);
            if (admin == null)
            {
                return Unauthorized("Administrador no encontrado.");
            }

            if (intentosFallidos.ContainsKey(admin.rol) && intentosFallidos[admin.rol] >= 5)
            {
                return Unauthorized("Cuenta bloqueada por múltiples intentos fallidos.");
            }

            if (administrador.cargo != admin.cargo) // Validar si el cargo o alguna otra propiedad es correcta
            {
                if (!intentosFallidos.ContainsKey(admin.rol))
                {
                    intentosFallidos[admin.rol] = 0;
                }
                intentosFallidos[admin.rol]++;
                return Unauthorized("Credenciales incorrectas.");
            }

            intentosFallidos[admin.rol] = 0; // Restablecer intentos al iniciar sesión con éxito.
            return Ok("Inicio de sesión exitoso.");
        }*/
    }
}
