using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Modelo;
using proyectoFinal.Data;
using CooperativaFinanciera.DTOs;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudCreditoController : ControllerBase
    {
        private readonly DBCconexion dBConexion;

        public SolicitudCreditoController(DBCconexion context)
        {
            dBConexion = context;
        }

        [HttpGet("Enlistar")]
        public async Task<ActionResult<List<SolicitudCredito>>> Get(
            string? estado,
            int? idTipoCredito,
            string? nombreCliente,
            bool ordenAscendente = true,
            string ordenarPor = "FechaSolicitud")
        {
            var query =dBConexion.SolicitudCredito
                .Include(s => s.Cliente)
                .ThenInclude(c => c.Usuario)
                .Include(s => s.TipoCredito)
                .Include(s => s.Parametro)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(s => s.estado == estado);
            }

            if (idTipoCredito.HasValue)
            {
                query = query.Where(s => s.idTipoCredito == idTipoCredito.Value);
            }

            if (!string.IsNullOrEmpty(nombreCliente))
            {
                query = query.Where(s => s.Cliente.Usuario.nombre.Contains(nombreCliente));
            }
            var solicitudesCredito = await query.ToListAsync();
            return Ok(solicitudesCredito);
        }

        [HttpPost("Insertar Nueva Solicitud Cred. S")]
        public async Task<ActionResult> Post([FromBody] SolicitudCredito solicitudCredito)
        {
            if (solicitudCredito == null)
            {
                return BadRequest("El objeto está vacío");
            }

            var idcliente = await dBConexion.Clientes.FindAsync(solicitudCredito.idCliente);
            if (idcliente == null)
            {
                return BadRequest("El id cliente no existe");
            }
            solicitudCredito.idCliente = idcliente.Id;

            var idtipocredito = await dBConexion.TipoCredito.FindAsync(solicitudCredito.idTipoCredito);
            if (idtipocredito == null)
            {
                return BadRequest("El id tipo credito no existe");
            }
            solicitudCredito.idTipoCredito = idtipocredito.idTipoCredito;

            var idparametro = await dBConexion.Parametros.FindAsync(solicitudCredito.idParametro);
            if (idparametro == null)
            {
                return BadRequest("El id parametro no existe");
            }
            solicitudCredito.idParametro = idparametro.Id;

            dBConexion.SolicitudCredito.Add(solicitudCredito);
            await dBConexion.SaveChangesAsync();
            return Ok("Se insertó correctamente");
        }

        [HttpPut("Modificar Solcitud Credito S")]
        public async Task<ActionResult> Update([FromBody] SolicitudCredito solicitudCredito, int idSolicitudCredito)
        {
            if (idSolicitudCredito == null)
            {
                return BadRequest("La solicitud de credito esta vacio");
            }

            if (idSolicitudCredito == 0)
            {
                return BadRequest("El id de solicitud de crédito está vacío");
            }

            var existeSolicitudCredito = await dBConexion.SolicitudCredito.FirstOrDefaultAsync(s => s.idSolicitudCredito == idSolicitudCredito);

            if (existeSolicitudCredito == null)
            {
                return NotFound("El id de solicitud de credito no existe");
            }

            // Modificar los campos
            existeSolicitudCredito.fechaSolicitud = solicitudCredito.fechaSolicitud;
            existeSolicitudCredito.estado = solicitudCredito.estado;
            existeSolicitudCredito.monto = solicitudCredito.monto;
            existeSolicitudCredito.ingresos = solicitudCredito.ingresos;
            existeSolicitudCredito.observaciones = solicitudCredito.observaciones;
            existeSolicitudCredito.idCliente = solicitudCredito.idCliente;
            existeSolicitudCredito.idTipoCredito = solicitudCredito.idTipoCredito;
            existeSolicitudCredito.idParametro = solicitudCredito.idParametro;

            await dBConexion.SaveChangesAsync();
            return Ok("Se modificaron los datos correctamente");
        }

        [HttpGet("pendingr")]
        public async Task<ActionResult<IEnumerable<SolicitudCreditoListDTO>>> GetPendingSolicitudes()
        {
            var solicitudes = await dBConexion.SolicitudCredito
                .Where(s => s.estado == "PENDIENTE")
                .Join(dBConexion.Clientes,
                    s => s.idCliente,
                    c => c.Id,
                    (s, c) => new { Solicitud = s, Cliente = c })
                .Join(dBConexion.Usuario,
                    sc => sc.Cliente.idUsuario,
                    u => u.idUsuario,
                    (sc, u) => new { sc.Solicitud, sc.Cliente, Usuario = u })
                .Join(dBConexion.TipoCredito,
                    scu => scu.Solicitud.idTipoCredito,
                    t => t.idTipoCredito,
                    (scu, t) => new SolicitudCreditoListDTO
                    {
                        idSolicitudCredito = scu.Solicitud.idSolicitudCredito,
                        fechaSolicitud = scu.Solicitud.fechaSolicitud,
                        estado = scu.Solicitud.estado,
                        monto = scu.Solicitud.monto,
                        nombreCliente = scu.Usuario.nombre,
                        tipoCredito = t.nombre
                    })
                .ToListAsync();

            return solicitudes;
        }

        // GET: api/solicitudcredito/5
        [HttpGet("r/{id}")]
        public async Task<ActionResult<SolicitudCreditoDetailDTO>> GetSolicitudCredito(int id)
        {
            // Obtener la solicitud
            var solicitud = await dBConexion.SolicitudCredito
                .Where(s => s.idSolicitudCredito == id)
                .Select(s => new
                {
                    s.idSolicitudCredito,
                    s.fechaSolicitud,
                    s.estado,
                    s.monto,
                    s.ingresos,
                    s.observaciones,
                    s.idCliente,
                    s.idTipoCredito,
                    s.idParametro
                })
                .FirstOrDefaultAsync();

            if (solicitud == null)
            {
                return NotFound();
            }

            // Obtener cliente y usuario
            var cliente = await dBConexion.Clientes
                .Where(c => c.Id == solicitud.idCliente)
                .Join(dBConexion.Usuario,
                    c => c.idUsuario,
                    u => u.idUsuario,
                    (c, u) => new ClienteDTO
                    {
                        idUsuario = u.idUsuario,
                        nombre = u.nombre,
                        email = u.email,
                        fechaNacimiento = c.FechaNacimiento,
                        genero = c.genero,
                        nacionalidad = c.nacionalidad,
                        telefono = c.Telefono,
                        direccion = c.direccion
                    })
                .FirstOrDefaultAsync();

            // Obtener tipo de crédito
            var tipoCredito = await dBConexion.TipoCredito
                .Where(t => t.idTipoCredito == solicitud.idTipoCredito)
                .Select(t => new TipoCreditoDTO
                {
                    idTipoCredito = t.idTipoCredito,
                    nombre = t.nombre,
                    descripcion = t.descripcion
                })
                .FirstOrDefaultAsync();

            // Obtener parámetros
            var parametro = await dBConexion.Parametros
                .Where(p => p.Id == solicitud.idParametro)
                .Select(p => new ParametroDTO
                {
                    idParametro = p.Id,
                    tasaInteres = p.TasaInteres,
                    plazo = p.PlazoMaximo,
                    formatoContrato = p.FormatoContrato
                })
                .FirstOrDefaultAsync();

            // Obtener documentos
            var documentos = await dBConexion.Documento
                .Where(d => d.idSolicitudCredito == solicitud.idSolicitudCredito)
                .Select(d => new DocumentoDTO
                {
                    idDocumento = d.idDocumento,
                    tipo = d.tipo,
                    fechaCarga = d.fechaCarga,
                    urlArchivo = $"/api/documento/{d.idDocumento}"
                })
                .ToListAsync();

            // Construir el DTO completo
            var solicitudDTO = new SolicitudCreditoDetailDTO
            {
                idSolicitudCredito = solicitud.idSolicitudCredito,
                fechaSolicitud = solicitud.fechaSolicitud,
                estado = solicitud.estado,
                monto = solicitud.monto,
                ingresos = solicitud.ingresos,
                observaciones = solicitud.observaciones,
                cliente = cliente,
                tipoCredito = tipoCredito,
                parametro = parametro,
                documentos = documentos
            };

            return solicitudDTO;
        }

        // PUT: api/solicitudcredito/5
        [HttpPut("r/{id}")]
        public async Task<IActionResult> UpdateSolicitudConditions(int id, ParametroDTO parametroDTO)
        {
            // Verificar que la solicitud existe
            var solicitudExists = await dBConexion.SolicitudCredito.AnyAsync(s => s.idSolicitudCredito == id);
            if (!solicitudExists)
            {
                return NotFound("Solicitud de crédito no encontrada");
            }

            // Obtener el parámetro directamente
            var parametro = await dBConexion.Parametros
                .FirstOrDefaultAsync(p => p.Id ==
                    dBConexion.SolicitudCredito
                        .Where(s => s.idSolicitudCredito == id)
                        .Select(s => s.idParametro)
                        .FirstOrDefault());

            if (parametro == null)
            {
                return NotFound("Parámetros no encontrados");
            }

            // Actualizar los parámetros
            parametro.TasaInteres = parametroDTO.tasaInteres;
            parametro.PlazoMaximo = parametroDTO.plazo;
            parametro.FormatoContrato = parametroDTO.formatoContrato;

            dBConexion.Entry(parametro).State = EntityState.Modified;

            try
            {
                await dBConexion.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await dBConexion.SolicitudCredito.AnyAsync(s => s.idSolicitudCredito == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        // PUT: api/solicitudcredito/5/status
        [HttpPut("{id}/statusr")]
        public async Task<IActionResult> UpdateSolicitudStatus(int id, SolicitudStatusDTO statusDTO)
        {
            var solicitud = await dBConexion.SolicitudCredito.FindAsync(id);

            if (solicitud == null)
            {
                return NotFound();
            }

            solicitud.estado = statusDTO.estado;
            dBConexion.Entry(solicitud).State = EntityState.Modified;

            try
            {
                await dBConexion.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await dBConexion.SolicitudCredito.AnyAsync(s => s.idSolicitudCredito == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool SolicitudCreditoExists(int id)
        {
            return dBConexion.SolicitudCredito.Any(e => e.idSolicitudCredito == id);
        }
    }
}
