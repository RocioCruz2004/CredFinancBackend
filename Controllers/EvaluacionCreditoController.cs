using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Modelo;
using proyectoFinal.Data;
using CooperativaFinanciera.DTOs;
using static iText.IO.Image.Jpeg2000ImageData;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionCreditoController : ControllerBase
    {
        private readonly DBCconexion dBConexion;

        public EvaluacionCreditoController(DBCconexion context)
        {
            dBConexion = context;
        }

        // POST: api/evaluacioncredito
        [HttpPost]
        public async Task<ActionResult<EvaluacionCreditoDTO>> CreateEvaluacion(EvaluacionCreditoDTO evaluacionDTO)
        {
            // Verificar que la solicitud existe sin cargar relaciones
            var solicitudExists = await dBConexion.SolicitudCredito
                .AnyAsync(s => s.idSolicitudCredito == evaluacionDTO.idSolicitudCredito);

            if (!solicitudExists)
            {
                return NotFound("Solicitud de crédito no encontrada");
            }

            // Verificar que el administrador existe sin cargar relaciones
            var adminExists = await dBConexion.Administrador
                .AnyAsync(a => a.idAdministrador == evaluacionDTO.idAdministrador);

            if (!adminExists)
            {
                return NotFound("Administrador no encontrado");
            }

            // Crear la evaluación
            var evaluacion = new EvaluacionCredito
            {
                decision = evaluacionDTO.decision,
                comentarios = evaluacionDTO.comentarios,
                fechaEvaluacion = DateTime.Now,
                idSolicitudCredito = evaluacionDTO.idSolicitudCredito,
                idAdministrador = evaluacionDTO.idAdministrador
            };

            dBConexion.EvaluacionCredito.Add(evaluacion);

            // Actualizar el estado de la solicitud
            var solicitud = await dBConexion.SolicitudCredito
                .Include(s => s.Cliente)
                .Include(s => s.Cliente.Usuario)
                .FirstOrDefaultAsync(s => s.idSolicitudCredito == evaluacionDTO.idSolicitudCredito);

            solicitud.estado = evaluacionDTO.decision;
            dBConexion.Entry(solicitud).State = EntityState.Modified;

            // Si la solicitud es aprobada, crear el crédito aprobado y el contrato
            if (evaluacionDTO.decision == "APROBADO")
            {
                // Obtener los parámetros actuales sin cargar relaciones
                var parametro = await dBConexion.Parametros
                    .FirstOrDefaultAsync(p => p.Id == solicitud.idParametro);

                if (parametro != null)
                {
                    // Crear el crédito aprobado
                    var creditoAprobado = new CreditoAprobado
                    {
                        FechaAprobacion = DateTime.Now,
                        MontoAprobado = solicitud.monto,
                        TasaAplicada = parametro.TasaInteres,
                        plazoAprobado = parametro.PlazoMaximo,
                        idSolicitudCredito = solicitud.idSolicitudCredito
                    };

                    dBConexion.CreditoAprobado.Add(creditoAprobado);

                    // Crear el contrato pendiente de firma
                    var contrato = new Contrato
                    {
                        fechaGeneracion = DateTime.Now,
                        estado = "GENERADO", // Estado inicial: GENERADO (pendiente de firma)
                        contenido = GenerarContenidoContrato(solicitud, parametro),
                        idSolicitudCredito = solicitud.idSolicitudCredito
                    };

                    dBConexion.Contratos.Add(contrato);

                    // Crear solo el primer pago.
                    DateTime fechaPrimerPago = DateTime.Now.AddMonths(1); // Primer pago en 1 mes.
                    var primerPago = new Pago
                    {
                        ClienteId = solicitud.idCliente,
                        FechaPago = fechaPrimerPago,
                        // Calcula el monto de la cuota dividiendo el monto total por el plazo, o asigna otro valor según la lógica del negocio.
                        Monto = solicitud.monto / parametro.PlazoMaximo,
                        Estado = EstadoPago.Pendiente
                    };

                    dBConexion.Pagos.Add(primerPago);
                }
            }

            await dBConexion.SaveChangesAsync();

            // Devolver DTO en lugar de la entidad
            var resultDTO = new EvaluacionCreditoDTO
            {
                decision = evaluacion.decision,
                comentarios = evaluacion.comentarios,
                idSolicitudCredito = evaluacion.idSolicitudCredito,
                idAdministrador = evaluacion.idAdministrador
            };

            return CreatedAtAction("GetEvaluacion", new { id = evaluacion.idEvaluacionCredito }, resultDTO);
        }

        // Método auxiliar para generar el contenido HTML del contrato
        private string GenerarContenidoContrato(SolicitudCredito solicitud, Parametro parametro)
        {
            // Obtener la fecha actual
            var fechaActual = DateTime.Now.ToString("dd/MM/yyyy");

            // Obtener información del cliente
            string nombreCliente = solicitud.Cliente?.Usuario?.nombre ?? "CLIENTE";

            // Generar el contenido HTML del contrato
            string contenidoHTML = $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset='UTF-8'>
        <title>Contrato de Crédito</title>
        <style>
            body {{ font-family: Arial, sans-serif; margin: 40px; line-height: 1.6; }}
            h1 {{ color: #003366; text-align: center; }}
            h2 {{ color: #003366; }}
            .header {{ text-align: center; margin-bottom: 30px; }}
            .footer {{ margin-top: 50px; text-align: center; }}
            .signature-area {{ margin-top: 50px; border-top: 1px solid #ccc; padding-top: 20px; }}
            .clause {{ margin-bottom: 20px; }}
            .important {{ font-weight: bold; }}
        </style>
    </head>
    <body>
        <div class='header'>
            <h1>CONTRATO DE CRÉDITO</h1>
            <p>Fecha: {fechaActual}</p>
        </div>

        <p>Entre los suscritos a saber, por una parte, <strong>COOPERATIVA FINANCIERA</strong>, entidad financiera legalmente constituida, representada en este acto por su representante legal, y por otra parte, <strong>{nombreCliente}</strong>, quien en adelante se denominará EL DEUDOR, hemos acordado celebrar el presente CONTRATO DE CRÉDITO, que se regirá por las siguientes cláusulas:</p>

        <div class='clause'>
            <h2>PRIMERA - OBJETO</h2>
            <p>COOPERATIVA FINANCIERA otorga a EL DEUDOR un crédito por la suma de <span class='important'>${solicitud.monto:N2} ({solicitud.monto} PESOS)</span>, que este declara haber recibido a su entera satisfacción.</p>
        </div>

        <div class='clause'>
            <h2>SEGUNDA - PLAZO Y FORMA DE PAGO</h2>
            <p>EL DEUDOR se obliga a pagar a COOPERATIVA FINANCIERA la suma indicada en la cláusula anterior, más los intereses correspondientes, en un plazo de <span class='important'>{parametro.PlazoMaximo} meses</span>, mediante cuotas mensuales consecutivas que incluyen capital e intereses.</p>
        </div>

        <div class='clause'>
            <h2>TERCERA - INTERESES</h2>
            <p>El crédito causará intereses a una tasa del <span class='important'>{parametro.TasaInteres}% anual</span> sobre saldos insolutos de capital.</p>
        </div>

        <div class='clause'>
            <h2>CUARTA - GARANTÍAS</h2>
            <p>Para garantizar el cumplimiento de las obligaciones derivadas del presente contrato, EL DEUDOR constituye a favor de COOPERATIVA FINANCIERA las siguientes garantías: [Detalles de garantías si aplica]</p>
        </div>

        <div class='clause'>
            <h2>QUINTA - INCUMPLIMIENTO</h2>
            <p>En caso de mora en el pago de una o más cuotas, COOPERATIVA FINANCIERA podrá declarar vencido el plazo y exigir el pago total de la obligación, más los intereses moratorios correspondientes.</p>
        </div>

        <div class='signature-area'>
            <p>Para constancia, se firma el presente contrato en dos ejemplares del mismo tenor, en la fecha indicada al inicio.</p>
            <div style='display: flex; justify-content: space-between; margin-top: 50px;'>
                <div style='width: 45%;'>
                    <p>_</p>
                    <p>COOPERATIVA FINANCIERA</p>
                    <p>Representante Legal</p>
                </div>
                <div style='width: 45%;'>
                    <p>_</p>
                    <p>EL DEUDOR</p>
                    <p>{nombreCliente}</p>
                </div>
            </div>
        </div>

        <div class='footer'>
            <p>Este contrato está sujeto a firma electrónica por parte del cliente.</p>
        </div>
    </body>
    </html>";

            return contenidoHTML;
        }

        // GET: api/evaluacioncredito/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluacionCreditoDTO>> GetEvaluacion(int id)
        {
            var evaluacion = await dBConexion.EvaluacionCredito
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.idEvaluacionCredito == id);

            if (evaluacion == null)
            {
                return NotFound();
            }

            // Devolver DTO en lugar de la entidad
            var evaluacionDTO = new EvaluacionCreditoDTO
            {
                decision = evaluacion.decision,
                comentarios = evaluacion.comentarios,
                idSolicitudCredito = evaluacion.idSolicitudCredito,
                idAdministrador = evaluacion.idAdministrador
            };

            return evaluacionDTO;
        }
    }
}