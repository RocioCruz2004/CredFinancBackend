using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Modelo;
using proyectoFinal.Data;
using CooperativaFinanciera.DTOs;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoController : ControllerBase
    {
        private readonly DBCconexion dBConexion;
        private readonly string _uploadsFolder;

        public DocumentoController(DBCconexion context, IWebHostEnvironment environment)
        {
            dBConexion = context;
            _uploadsFolder = Path.Combine(environment.ContentRootPath, "Uploads");

            // Asegurar que el directorio de uploads exista
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        // GET: api/documento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentoDTO>> GetDocumento(int id)
        {
            var documento = await dBConexion.Documento
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.idDocumento == id);

            if (documento == null)
            {
                return NotFound();
            }

            // Devolver DTO en lugar de la entidad
            var documentoDTO = new DocumentoDTO
            {
                idDocumento = documento.idDocumento,
                tipo = documento.tipo,
                fechaCarga = documento.fechaCarga,
                urlArchivo = $"/api/documento/download/{documento.idDocumento}"
            };

            return documentoDTO;
        }

        // GET: api/documento/download/5
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadDocumento(int id)
        {
            var documento = await dBConexion.Documento.FindAsync(id);

            if (documento == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_uploadsFolder, documento.urlArchivo);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Archivo no encontrado");
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/octet-stream", Path.GetFileName(documento.urlArchivo));
        }

        public class DocumentoInputModel
        {
            public IFormFile File { get; set; }
            public int idSolicitudCredito { get; set; }
            public string Tipo { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult<DocumentoDTO>> CreateDocumento([FromForm] DocumentoInputModel input)
        {
            // Verificar que la solicitud existe
            var solicitud = await dBConexion.SolicitudCredito.FindAsync(input.idSolicitudCredito);
            if (solicitud == null)
            {
                return NotFound("Solicitud de crédito no encontrada");
            }

            if (input.File == null || input.File.Length == 0)
            {
                return BadRequest("No se ha proporcionado un archivo válido");
            }

            // Generar un nombre único para el archivo
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(input.File.FileName)}";
            var filePath = Path.Combine(_uploadsFolder, fileName);

            // Guardar el archivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await input.File.CopyToAsync(stream);
            }

            // Crear el documento en la base de datos
            var documento = new Documento
            {
                tipo = input.Tipo,
                fechaCarga = DateTime.Now,
                urlArchivo = fileName,
                idSolicitudCredito = input.idSolicitudCredito
            };

            dBConexion.Documento.Add(documento);
            await dBConexion.SaveChangesAsync();

            // Devolver DTO en lugar de la entidad
            var documentoDTO = new DocumentoDTO
            {
                idDocumento = documento.idDocumento,
                tipo = documento.tipo,
                fechaCarga = documento.fechaCarga,
                urlArchivo = $"/api/documento/download/{documento.idDocumento}"
            };

            return CreatedAtAction("GetDocumento", new { id = documento.idDocumento }, documentoDTO);
        }

        // DELETE: api/documento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            var documento = await dBConexion.Documento.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }

            // Eliminar el archivo físico
            var filePath = Path.Combine(_uploadsFolder, documento.urlArchivo);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Eliminar el registro de la base de datos
            dBConexion.Documento.Remove(documento);
            await dBConexion.SaveChangesAsync();

            return NoContent();
        }
    }
}
