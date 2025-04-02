using CooperativaFinanciera.DTOs;
using HistoriaUsuario2.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Data;
using proyectoFinal.DTOs;
using proyectoFinal.Modelo;
using static proyectoFinal.ViewModels.ViewModel;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudClienteController : ControllerBase
    {
        private readonly DBCconexion _dbContext;

        public SolicitudClienteController(DBCconexion dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(SolicitudCreditoClienteDTO solicitudDto)
        {
            var tipoCreditoExist = await _dbContext.TipoCredito
                .AnyAsync(t => t.idTipoCredito == solicitudDto.idTipoCredito);
            if (!tipoCreditoExist)
            {
                return BadRequest("El tipo de credito no existe");
            }

            var solicitud = new SolicitudCredito
            {
                fechaSolicitud = DateTime.Now,
                estado = "Pendiente",
                monto = solicitudDto.monto,
                ingresos = solicitudDto.ingresos,
                observaciones = solicitudDto.observaciones,
                idTipoCredito = solicitudDto.idTipoCredito,
                idCliente = solicitudDto.idCliente,
                idParametro = solicitudDto.idParametro
            };

            _dbContext.SolicitudCredito.Add(solicitud);
            await _dbContext.SaveChangesAsync();

            return Ok("Solicitud Enviada");
        }

        [HttpPost("GuardarDocumento")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> GuardarDocumento([FromForm] DocumentoClienteDTO documentoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var solicitudExistente = await _dbContext.SolicitudCredito
                .AsNoTracking()
                .AnyAsync(s => s.idSolicitudCredito == documentoDto.idSolicitudCredito);

            if (!solicitudExistente)
            {
                return NotFound($"No se encontró la solicitud de crédito con ID {documentoDto.idSolicitudCredito}");
            }

            if (documentoDto.archivo == null || documentoDto.archivo.Length == 0)
            {
                return BadRequest("No se ha proporcionado un archivo válido");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(documentoDto.archivo.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            var relativePath = Path.Combine("Uploads", fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await documentoDto.archivo.CopyToAsync(stream);
            }

            var documento = new Documento
            {
                tipo = documentoDto.tipo,
                urlArchivo = fileName,
                fechaCarga = DateTime.UtcNow,
                idSolicitudCredito = documentoDto.idSolicitudCredito
            };
            await _dbContext.Documento.AddAsync(documento);
            await _dbContext.SaveChangesAsync();

            return Ok("Documento creado exitosamente");
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<List<SolicitudCreditoViewModel>>> GetSolicitudesPorCliente(int idCliente)
        {
            var solicitudes = await _dbContext.SolicitudCredito
                .Include(s => s.TipoCredito)
                .Where(s => s.idCliente == idCliente)
                .ToListAsync();

            if (!solicitudes.Any())
            {
                return NotFound("No se encontraron solicitudes para este cliente");
            }

            var response = new List<SolicitudCreditoViewModel>();

            foreach (var solicitud in solicitudes)
            {
                var documentos = await _dbContext.Documento
                    .Where(d => d.idSolicitudCredito == solicitud.idSolicitudCredito)
                    .ToListAsync();

                response.Add(new SolicitudCreditoViewModel
                {
                    FechaSolicitud = solicitud.fechaSolicitud,
                    Estado = solicitud.estado,
                    Monto = solicitud.monto,
                    Ingresos = solicitud.ingresos,
                    Observaciones = solicitud.observaciones,
                    TipoCredito = new TipoCreditoViewModel
                    {
                        Nombre = solicitud.TipoCredito?.nombre,
                        Descripcion = solicitud.TipoCredito?.descripcion
                    },
                    Documentos = documentos.Select(d => new DocumentoViewModel
                    {
                        Tipo = d.tipo,
                        ArchivoUrl = $"/Uploads/{Path.GetFileName(d.urlArchivo)}",
                        FechaCarga = d.fechaCarga.ToString("dd/MM/yyyy HH:mm")
                    }).ToList()
                });
            }

            return Ok(response);
        }

        [HttpGet("ultimaSolicitudId")]
        public async Task<ActionResult<int>> GetUltimaSolicitudId()
        {
            var ultimaSolicitudId = await _dbContext.SolicitudCredito
                .OrderByDescending(s => s.idSolicitudCredito)
                .Select(s => s.idSolicitudCredito)
                .FirstOrDefaultAsync();

            if (ultimaSolicitudId == 0)
            {
                return NotFound("No se encontraron solicitudes de crédito");
            }

            return Ok(ultimaSolicitudId);
        }
    }
}