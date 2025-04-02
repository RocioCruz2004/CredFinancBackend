using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Data;
using proyectoFinal.Modelo;
using proyectoFinal.DTOs;
using System.Security.Cryptography;
using System.Text;
using CooperativaFinanciera.Services;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoController : ControllerBase
    {
        private readonly DBCconexion _context;
        private readonly INotificacionService _notificacionService;
        private readonly Random _random = new Random();

        public ContratoController(DBCconexion context, INotificacionService notificacionService)
        {
            _context = context;
            _notificacionService = notificacionService;
        }

        // GET: api/Contrato
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contrato>>> GetContratos()
        {
            return await _context.Contratos.ToListAsync();
        }

        // GET: api/Contrato/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContratoDTO>> GetContrato(int id)
        {
            var contrato = await _context.Contratos
                .Where(c => c.id == id)
                .Include(c => c.SolicitudCredito)
                .ThenInclude(s => s.Cliente)
                .ThenInclude(c => c.Usuario)
                .Include(c => c.SolicitudCredito.CreditoAprobado)
                .FirstOrDefaultAsync();

            if (contrato == null)
            {
                return NotFound("Contrato no encontrado");
            }

            var contratoDTO = new ContratoDTO
            {
                id = contrato.id,
                fechaGeneracion = contrato.fechaGeneracion,
                fechaFirma = contrato.fechaFirma,
                contenido = contrato.contenido,
                estado = contrato.estado,
                firmaDigital = contrato.firmaDigital,
                idSolicitudCredito = contrato.idSolicitudCredito,
                nombreCliente = contrato.SolicitudCredito.Cliente.Usuario.nombre,
                montoAprobado = contrato.SolicitudCredito.CreditoAprobado != null ? contrato.SolicitudCredito.CreditoAprobado.MontoAprobado : 0,
                tasaInteres = contrato.SolicitudCredito.CreditoAprobado != null ? contrato.SolicitudCredito.CreditoAprobado.TasaAplicada : 0,
                plazo = contrato.SolicitudCredito.CreditoAprobado != null ? contrato.SolicitudCredito.CreditoAprobado.plazoAprobado : 0
            };

            return contratoDTO;
        }

        // PUT: api/Contrato/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContrato(int id, Contrato contrato)
        {
            if (id != contrato.id)
            {
                return BadRequest();
            }

            _context.Entry(contrato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContratoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contrato
        [HttpPost]
        public async Task<ActionResult<Contrato>> PostContrato(Contrato contrato)
        {
            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContrato", new { id = contrato.id }, contrato);
        }

        // DELETE: api/Contrato/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContrato(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null)
            {
                return NotFound();
            }

            _context.Contratos.Remove(contrato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContratoExists(int id)
        {
            return _context.Contratos.Any(e => e.id == id);
        }

        private string GenerarCodigoVerificacion()
        {
            // Genera un número aleatorio de 6 dígitos
            return _random.Next(100000, 999999).ToString();
        }

        // POST: api/contrato/auth/doblefactor
        // Validar la autenticación de doble factor del Cliente
        [HttpPost("auth/doblefactor")]
        public async Task<ActionResult<RespuestaDobleFactor>> SolicitarDobleFactor(SolicitudDobleFactor solicitud)
        {
            var contrato = await _context.Contratos
                .Include(c => c.SolicitudCredito)
                .ThenInclude(s => s.Cliente)
                .ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.id == solicitud.idContrato);

            if (contrato == null)
            {
                return NotFound("Contrato no encontrado");
            }

            if (contrato.estado != "GENERADO")
            {
                return BadRequest("El contrato no está en estado válido para firma");
            }

            // Generar código de verificación (6 dígitos)
            string codigoVerificacion = GenerarCodigoVerificacion();

            // Almacenar método de autenticación y código de verificación
            contrato.metodoAutenticacion = solicitud.metodo;
            contrato.codigoVerificacion = codigoVerificacion;
            contrato.estado = "PENDIENTE_FIRMA";

            _context.Entry(contrato).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Obtener información del cliente
            string emailCliente = contrato.SolicitudCredito.Cliente.Usuario.email;
            string nombreCliente = contrato.SolicitudCredito.Cliente.Usuario.nombre;

            // Enviar el código por el método seleccionado
            bool notificacionEnviada = false;
            string mensajeRespuesta = "";

            if (solicitud.metodo == "EMAIL")
            {
                // Crear el mensaje HTML para el correo
                string asunto = "Código de verificación para firma de contrato";
                string cuerpoMensaje = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #0066cc; color: white; padding: 10px 20px; text-align: center; }}
                    .content {{ padding: 20px; border: 1px solid #ddd; }}
                    .code {{ font-size: 24px; font-weight: bold; text-align: center; margin: 20px 0; color: #0066cc; letter-spacing: 5px; }}
                    .footer {{ font-size: 12px; text-align: center; margin-top: 20px; color: #666; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Cooperativa Financiera</h2>
                    </div>
                    <div class='content'>
                        <p>Estimado(a) <strong>{nombreCliente}</strong>,</p>
                        <p>Hemos recibido una solicitud para firmar un contrato asociado a su solicitud de crédito. Para completar este proceso, por favor utilice el siguiente código de verificación:</p>
                        <div class='code'>{codigoVerificacion}</div>
                        <p>Este código es válido por 15 minutos. Si usted no ha solicitado firmar ningún contrato, por favor ignore este mensaje.</p>
                        <p>Gracias por confiar en nosotros.</p>
                    </div>
                    <div class='footer'>
                        <p>Este es un mensaje automático, por favor no responda a este correo.</p>
                        <p>&copy; {DateTime.Now.Year} Cooperativa Financiera. Todos los derechos reservados.</p>
                    </div>
                </div>
            </body>
            </html>";

                notificacionEnviada = await _notificacionService.EnviarNotificacionEmail(emailCliente, asunto, cuerpoMensaje);
                mensajeRespuesta = notificacionEnviada
                    ? $"Se ha enviado un código de verificación a su correo electrónico {emailCliente}"
                    : "No se pudo enviar el código de verificación. Por favor, intente nuevamente.";
            }
            else if (solicitud.metodo == "SMS")
            {
                // En un entorno real, aquí se integraría con un servicio de SMS
                // Para este ejemplo, simulamos que el envío fue exitoso
                notificacionEnviada = true;
                mensajeRespuesta = "Se ha enviado un código de verificación a su teléfono móvil";

                // Registrar en consola para propósitos de desarrollo
                Console.WriteLine($"[SIMULACIÓN SMS] Enviando código {codigoVerificacion} a {nombreCliente}");
            }

            // Crear notificación en el sistema
            var notificacion = new Notificacion
            {
                mensaje = $"Se ha enviado un código de verificación para la firma del contrato de su solicitud #{contrato.idSolicitudCredito}.",
                fechaEnvio = DateTime.Now,
                tipo = "CODIGO_VERIFICACION",
                idSolicitudCredito = contrato.idSolicitudCredito
            };

            _context.Notificacion.Add(notificacion);
            await _context.SaveChangesAsync();

            // Por seguridad, no devolvemos el código real en la respuesta
            return new RespuestaDobleFactor
            {
                exito = notificacionEnviada,
                mensaje = mensajeRespuesta,
                codigoVerificacion = "******" // Ocultamos el código real
            };
        }

        // GET: api/contrato/cliente/{idCliente}
        // Recuperar los contratos de un cliente específico
        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<IEnumerable<ContratoDTO>>> GetContratosByCliente(int idCliente)
        {
            var contratos = await _context.Contratos
                .Where(c => c.SolicitudCredito.idCliente == idCliente)
                .Include(c => c.SolicitudCredito)
                .ThenInclude(s => s.Cliente)
                .ThenInclude(c => c.Usuario)
                .Include(c => c.SolicitudCredito.CreditoAprobado)
                .ToListAsync();

            if (contratos == null || !contratos.Any())
            {
                return new List<ContratoDTO>();
            }

            var contratosDTO = contratos.Select(contrato => new ContratoDTO
            {
                id = contrato.id,
                fechaGeneracion = contrato.fechaGeneracion,
                fechaFirma = contrato.fechaFirma,
                contenido = contrato.contenido,
                estado = contrato.estado,
                firmaDigital = contrato.firmaDigital,
                idSolicitudCredito = contrato.idSolicitudCredito,
                nombreCliente = contrato.SolicitudCredito.Cliente.Usuario.nombre,
                montoAprobado = contrato.SolicitudCredito.CreditoAprobado != null ? contrato.SolicitudCredito.CreditoAprobado.MontoAprobado : 0,
                tasaInteres = contrato.SolicitudCredito.CreditoAprobado != null ? contrato.SolicitudCredito.CreditoAprobado.TasaAplicada : 0,
                plazo = contrato.SolicitudCredito.CreditoAprobado != null ? contrato.SolicitudCredito.CreditoAprobado.plazoAprobado : 0
            }).ToList();

            return contratosDTO;
        }

        // POST: api/contrato/firmar
        // Registrar la firma digital del contrato
        [HttpPost("firmar")]
        public async Task<ActionResult<RespuestaFirmaDTO>> FirmarContrato(FirmaContratoDTO firmaDTO)
        {
            var contrato = await _context.Contratos.FindAsync(firmaDTO.idContrato);

            if (contrato == null)
            {
                return NotFound("Contrato no encontrado");
            }

            if (contrato.estado != "PENDIENTE_FIRMA")
            {
                return BadRequest("El contrato no está en estado válido para firma");
            }

            // Verificar el código de autenticación
            if (contrato.codigoVerificacion != firmaDTO.codigoVerificacion)
            {
                return BadRequest(new RespuestaFirmaDTO
                {
                    exito = false,
                    mensaje = "Código de verificación incorrecto",
                    estado = contrato.estado,
                    fechaFirma = null
                });
            }

            // Generar firma digital (en un entorno real, esto sería más complejo)
            string firmaDigital = GenerarFirmaDigital(contrato.id, contrato.idSolicitudCredito);

            // Actualizar el contrato
            contrato.estado = "FIRMADO";
            contrato.fechaFirma = DateTime.Now;
            contrato.firmaDigital = firmaDigital;

            _context.Entry(contrato).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Crear notificación de contrato firmado
            var notificacion = new Notificacion
            {
                mensaje = $"El contrato para su solicitud de crédito #{contrato.idSolicitudCredito} ha sido firmado exitosamente.",
                fechaEnvio = DateTime.Now,
                tipo = "CONTRATO_FIRMADO",
                idSolicitudCredito = contrato.idSolicitudCredito
            };

            _context.Notificacion.Add(notificacion);
            await _context.SaveChangesAsync();

           

            return new RespuestaFirmaDTO
            {
                exito = true,
                mensaje = "Contrato firmado exitosamente",
                estado = contrato.estado,
                fechaFirma = contrato.fechaFirma
            };
        }

        private string GenerarFirmaDigital(int idContrato, int idSolicitud)
        {
            // En un entorno real, esto sería un proceso criptográfico más complejo
            // Aquí simplemente generamos un hash para simular una firma digital
            string datos = $"{idContrato}-{idSolicitud}-{DateTime.Now.Ticks}";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(datos));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // GET: api/contrato/ver/{id}
        [HttpGet("ver/{id}")]
        public async Task<ActionResult<ContratoVisualizacionDTO>> VerContrato(int id)
        {
            var contrato = await _context.Contratos
                .Include(c => c.SolicitudCredito)
                .Include(c => c.SolicitudCredito.Cliente)
                .Include(c => c.SolicitudCredito.Cliente.Usuario)
                .FirstOrDefaultAsync(c => c.id == id);

            if (contrato == null)
            {
                return NotFound("Contrato no encontrado");
            }

            // Crear un DTO específico para visualización
            var contratoDTO = new ContratoVisualizacionDTO
            {
                Id = contrato.id,
                FechaGeneracion = contrato.fechaGeneracion,
                FechaFirma = contrato.fechaFirma,
                Contenido = contrato.contenido,
                Estado = contrato.estado,
                IdSolicitudCredito = contrato.idSolicitudCredito,
                NombreCliente = contrato.SolicitudCredito.Cliente.Usuario.nombre,
                EstaFirmado = contrato.estado == "FIRMADO"
            };

            return contratoDTO;
        }
    }
}

