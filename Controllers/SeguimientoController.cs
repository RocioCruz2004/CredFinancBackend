using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Modelo;
using proyectoFinal.DTOs;
using proyectoFinal.Data;
using CooperativaFinanciera.DTOs;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguimientoController : ControllerBase
    {
        private readonly DBCconexion _context;

        public SeguimientoController(DBCconexion context)
        {
            _context = context;
        }

        // GET: api/Seguimiento/solicitudes/{clienteId}
        [HttpGet("solicitudes/{clienteId}")]
        public async Task<ActionResult<IEnumerable<SolicitudCreditoListDTO>>> GetSolicitudesByCliente(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado");
            }

            var solicitudes = await _context.SolicitudCredito
                .Where(s => s.idCliente == clienteId)
                .Include(s => s.Cliente)
                .ThenInclude(c => c.Usuario)
                .Include(s => s.TipoCredito)
                .Select(s => new SolicitudCreditoListDTO
                {
                    idSolicitudCredito = s.idSolicitudCredito,
                    fechaSolicitud = s.fechaSolicitud,
                    estado = s.estado,
                    monto = s.monto,
                    nombreCliente = s.Cliente.Usuario.nombre,
                    tipoCredito = s.TipoCredito.nombre
                })
                .ToListAsync();

            return solicitudes;
        }

        // GET: api/Seguimiento/pagos/{solicitudId}
        [HttpGet("pagos/{solicitudId}")]
        public async Task<ActionResult<IEnumerable<PagoDTO>>> GetPagosBySolicitud(int solicitudId)
        {
            var solicitud = await _context.SolicitudCredito
                .Include(s => s.Cliente)
                .Include(s => s.CreditoAprobado)
                .FirstOrDefaultAsync(s => s.idSolicitudCredito == solicitudId);

            if (solicitud == null)
            {
                return NotFound("Solicitud de crédito no encontrada");
            }

            if (solicitud.CreditoAprobado == null)
            {
                return NotFound("Esta solicitud no tiene un crédito aprobado asociado");
            }

            var pagos = await _context.Pagos
                .Where(p => p.ClienteId == solicitud.idCliente)
                .Select(p => new PagoDTO
                {
                    Id = p.Id,
                    FechaPago = p.FechaPago,
                    Monto = p.Monto,
                    Estado = p.Estado.ToString(),
                    SolicitudId = solicitudId
                })
                .ToListAsync();

            return pagos;
        }

        // GET: api/Seguimiento/pago/{pagoId}
        [HttpGet("pago/{pagoId}")]
        public async Task<ActionResult<PagoDetalleDTO>> GetPagoDetalle(int pagoId)
        {
            var pago = await _context.Pagos
                .Include(p => p.Cliente)
                .ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(p => p.Id == pagoId);

            if (pago == null)
            {
                return NotFound("Pago no encontrado");
            }

            // Buscar la solicitud relacionada con este cliente
            var solicitud = await _context.SolicitudCredito
                .Include(s => s.CreditoAprobado)
                .FirstOrDefaultAsync(s => s.idCliente == pago.ClienteId);

            if (solicitud == null)
            {
                return NotFound("No se encontró una solicitud asociada a este pago");
            }

            var pagoDetalle = new PagoDetalleDTO
            {
                Id = pago.Id,
                FechaPago = pago.FechaPago,
                Monto = pago.Monto,
                Estado = pago.Estado.ToString(),
                ClienteNombre = pago.Cliente.Usuario.nombre,
                SolicitudId = solicitud.idSolicitudCredito,
                MontoCredito = solicitud.CreditoAprobado?.MontoAprobado ?? 0,
                TasaAplicada = solicitud.CreditoAprobado?.TasaAplicada ?? 0,
                PlazoAprobado = solicitud.CreditoAprobado?.plazoAprobado ?? 0
            };

            return pagoDetalle;
        }

        // GET: api/Seguimiento/alertas/{solicitudId}
        [HttpGet("alertas/{solicitudId}")]
        public async Task<ActionResult<IEnumerable<AlertaDTO>>> GetAlertasBySolicitud(int solicitudId)
        {
            var solicitud = await _context.SolicitudCredito.FindAsync(solicitudId);
            if (solicitud == null)
            {
                return NotFound("Solicitud de crédito no encontrada");
            }

            // Obtener pagos atrasados
            var pagosAtrasados = await _context.Pagos
                .Where(p => p.ClienteId == solicitud.idCliente && p.Estado == EstadoPago.Atrasado)
                .ToListAsync();

            // Obtener notificaciones existentes
            var notificaciones = await _context.Notificacion
                .Where(n => n.idSolicitudCredito == solicitudId && n.tipo.Contains("Alerta"))
                .ToListAsync();

            var alertas = new List<AlertaDTO>();

            // Convertir pagos atrasados a alertas
            foreach (var pago in pagosAtrasados)
            {
                alertas.Add(new AlertaDTO
                {
                    Tipo = "PagoAtrasado",
                    Mensaje = $"El pago programado para {pago.FechaPago.ToShortDateString()} está atrasado",
                    Fecha = pago.FechaPago,
                    SolicitudId = solicitudId,
                    PagoId = pago.Id
                });
            }

            // Convertir notificaciones a alertas
            foreach (var notificacion in notificaciones)
            {
                alertas.Add(new AlertaDTO
                {
                    Tipo = notificacion.tipo,
                    Mensaje = notificacion.mensaje,
                    Fecha = notificacion.fechaEnvio,
                    SolicitudId = solicitudId
                });
            }

            return alertas.OrderByDescending(a => a.Fecha).ToList();
        }

        // GET: api/Seguimiento/reporte/{clienteId}
        [HttpGet("reporte/{clienteId}")]
        public async Task<ActionResult<ReporteFinancieroDTO>> GetReporteFinanciero(int clienteId)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.SolicitudesCredito)
                .ThenInclude(s => s.CreditoAprobado)
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            if (cliente == null)
            {
                return NotFound("Cliente no encontrado");
            }

            // Obtener todos los pagos del cliente
            var pagos = await _context.Pagos
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();

            // Calcular totales
            double totalPagado = pagos.Where(p => p.Estado == EstadoPago.Pagado).Sum(p => p.Monto);
            double totalMorosidad = pagos.Where(p => p.Estado == EstadoPago.Atrasado).Sum(p => p.Monto);
            int pagosPendientes = pagos.Count(p => p.Estado == EstadoPago.Pendiente);
            int pagosAtrasados = pagos.Count(p => p.Estado == EstadoPago.Atrasado);

            // Obtener solicitudes activas
            var solicitudesActivas = cliente.SolicitudesCredito
                .Where(s => s.estado == "Aprobada" || s.estado == "En Proceso")
                .Count();

            // Crear reporte
            var reporte = new ReporteFinancieroDTO
            {
                ClienteId = clienteId,
                NombreCliente = cliente.Usuario.nombre,
                FechaGeneracion = DateTime.Now,
                TotalPagado = totalPagado,
                TotalMorosidad = totalMorosidad,
                PagosPendientes = pagosPendientes,
                PagosAtrasados = pagosAtrasados,
                SolicitudesActivas = solicitudesActivas,
                HistorialPagos = pagos.OrderByDescending(p => p.FechaPago)
                    .Select(p => new PagoDTO
                    {
                        Id = p.Id,
                        FechaPago = p.FechaPago,
                        Monto = p.Monto,
                        Estado = p.Estado.ToString()
                    }).ToList()
            };

            // Guardar el reporte en la base de datos
            var reporteEntity = new ReporteFinanciero
            {
                ClienteId = clienteId,
                FechaGeneracion = DateTime.Now,
                TotalPagado = totalPagado,
                TotalMorosidad = totalMorosidad
            };

            _context.ReportesFinancieros.Add(reporteEntity);
            await _context.SaveChangesAsync();

            return reporte;
        }
    }
}