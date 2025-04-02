using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Modelo;
using proyectoFinal.Data;
using CooperativaFinanciera.DTOs;
using CooperativaFinanciera.Services;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly DBCconexion dBConexion;
        private readonly INotificacionService _notificacionService;

        public NotificacionController(DBCconexion context, INotificacionService notificacionService)
        {
            dBConexion = context;
            _notificacionService = notificacionService;
        }

        // POST: api/notificacion
        [HttpPost]
        public async Task<ActionResult<NotificacionDTO>> CreateNotificacion(NotificacionDTO notificacionDTO)
        {
            // Verificar que la solicitud existe y obtener el email del cliente
            var solicitudInfo = await dBConexion.SolicitudCredito
                .Join(dBConexion.Clientes,
                    s => s.idCliente,
                    c => c.Id,
                    (s, c) => new { s.idSolicitudCredito, c.idUsuario })
                .Join(dBConexion.Usuario,
                    sc => sc.idUsuario,
                    u => u.idUsuario,
                    (sc, u) => new { sc.idSolicitudCredito, Email = u.email })
                .FirstOrDefaultAsync(s => s.idSolicitudCredito == notificacionDTO.idSolicitudCredito);

            if (solicitudInfo == null)
            {
                return NotFound("Solicitud de crédito no encontrada");
            }

            // Verificar que el crédito aprobado existe si se proporciona un ID
            if (notificacionDTO.idCreditoAprobado.HasValue)
            {
                var creditoExists = await dBConexion.CreditoAprobado
                    .AnyAsync(c => c.Id == notificacionDTO.idCreditoAprobado.Value);

                if (!creditoExists)
                {
                    return NotFound("Crédito aprobado no encontrado");
                }
            }

            // Crear la notificación
            var notificacion = new Notificacion
            {
                mensaje = notificacionDTO.mensaje,
                fechaEnvio = DateTime.Now,
                tipo = notificacionDTO.tipo,
                idSolicitudCredito = notificacionDTO.idSolicitudCredito,
                idCreditoAprobado = notificacionDTO.idCreditoAprobado // Puede ser null
            };

            dBConexion.Notificacion.Add(notificacion);
            await dBConexion.SaveChangesAsync();

            // Enviar la notificación al cliente
            await _notificacionService.EnviarNotificacionEmail(
                solicitudInfo.Email,
                $"Actualización de su solicitud de crédito #{notificacionDTO.idSolicitudCredito}",
                notificacionDTO.mensaje
            );

            // Devolver DTO en lugar de la entidad
            var resultDTO = new NotificacionDTO
            {
                mensaje = notificacion.mensaje,
                tipo = notificacion.tipo,
                idSolicitudCredito = notificacion.idSolicitudCredito,
                idCreditoAprobado = notificacion.idCreditoAprobado
            };

            return CreatedAtAction("GetNotificacion", new { id = notificacion.idNotificacion }, resultDTO);
        }

        // GET: api/notificacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificacionDTO>> GetNotificacion(int id)
        {
            var notificacion = await dBConexion.Notificacion
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.idNotificacion == id);

            if (notificacion == null)
            {
                return NotFound();
            }

            // Devolver DTO en lugar de la entidad
            var notificacionDTO = new NotificacionDTO
            {
                mensaje = notificacion.mensaje,
                tipo = notificacion.tipo,
                idSolicitudCredito = notificacion.idSolicitudCredito,
                idCreditoAprobado = notificacion.idCreditoAprobado
            };

            return notificacionDTO;
        }
    }
}
