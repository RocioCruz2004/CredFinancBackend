using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Modelo;
using CooperativaFinanciera.DTOs;
using proyectoFinal.Data;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly DBCconexion dBCconexion;

        public AuditLogController(DBCconexion context)
        {
            dBCconexion = context;
        }

        // POST: api/audit
        [HttpPost]
        public async Task<ActionResult<AuditLogDTO>> CreateAuditLog(AuditLogDTO auditDTO)
        {
            // Verificar que el administrador existe y obtener su nombre
            var admin = await dBCconexion.Administrador
                .Join(dBCconexion.Usuario,
                    a => a.idUsuario,
                    u => u.idUsuario,
                    (a, u) => new { a.idAdministrador, Nombre = u.nombre })
                .FirstOrDefaultAsync(a => a.idAdministrador == auditDTO.idAdministrador);

            if (admin == null)
            {
                return NotFound("Administrador no encontrado");
            }

            var auditLog = new AuditLog
            {
                accion = auditDTO.accion,
                usuario = admin.Nombre,
                fecha = DateTime.Now,
                detalles = auditDTO.detalles,
                idAdministrador = auditDTO.idAdministrador
            };

            dBCconexion.AuditLog.Add(auditLog);
            await dBCconexion.SaveChangesAsync();

            // Devolver DTO en lugar de la entidad
            var resultDTO = new AuditLogDTO
            {
                accion = auditLog.accion,
                detalles = auditLog.detalles,
                idAdministrador = auditLog.idAdministrador
            };

            return CreatedAtAction("GetAuditLog", new { id = auditLog.idAuditLog }, resultDTO);
        }

        // GET: api/audit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuditLogDTO>> GetAuditLog(int id)
        {
            var auditLog = await dBCconexion.AuditLog
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.idAuditLog == id);

            if (auditLog == null)
            {
                return NotFound();
            }

            // Devolver DTO en lugar de la entidad
            var auditLogDTO = new AuditLogDTO
            {
                accion = auditLog.accion,
                detalles = auditLog.detalles,
                idAdministrador = auditLog.idAdministrador
            };

            return auditLogDTO;
        }
    }
}
