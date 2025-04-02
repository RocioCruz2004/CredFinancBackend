using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Data;
using proyectoFinal.Modelo;
using OfficeOpenXml;  // Para exportar a Excel
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteFinancieroController : ControllerBase
    {
        private readonly DBCconexion _context;

        public ReporteFinancieroController(DBCconexion db)
        {
            _context = db;
        }

        // Obtener reportes financieros con filtros de fecha y estado
        [HttpGet]
        public async Task<ActionResult<List<ReporteFinanciero>>> GetReportes(
    [FromQuery] DateTime? fechaInicio,
    [FromQuery] DateTime? fechaFin,
    [FromQuery] string? estado)
        {
            var reportes = _context.ReportesFinancieros
                .Include(r => r.Cliente)
                .ThenInclude(c => c.Usuario) // Añade esta línea
                .Include(r => r.Cliente.Pagos)
                .AsQueryable();

            // Filtrar por fecha de generación
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                reportes = reportes.Where(r => r.FechaGeneracion >= fechaInicio && r.FechaGeneracion <= fechaFin);
            }
            else if (fechaInicio.HasValue)
            {
                reportes = reportes.Where(r => r.FechaGeneracion >= fechaInicio);
            }
            else if (fechaFin.HasValue)
            {
                reportes = reportes.Where(r => r.FechaGeneracion <= fechaFin);
            }

            // Filtrar por estado de pago
            if (!string.IsNullOrEmpty(estado))
            {
                EstadoPago estadoPago;
                if (Enum.TryParse(estado, true, out estadoPago))
                {
                    reportes = reportes.Where(r => r.Cliente.Pagos.Any(p => p.Estado == estadoPago));
                }
                else
                {
                    return BadRequest("El estado especificado no es válido.");
                }
            }

            var listaReportes = await reportes.ToListAsync();

            if (listaReportes == null || !listaReportes.Any())
                return NotFound("No se encontraron reportes financieros para los filtros especificados.");

            return Ok(listaReportes);
        }
    }
}