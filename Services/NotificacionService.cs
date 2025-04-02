using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CooperativaFinanciera.Services
{
    public interface INotificacionService
    {
        Task<bool> EnviarNotificacionEmail(string destinatario, string asunto, string mensaje);
    }

    public class NotificacionService : INotificacionService
    {
        private readonly IConfiguration _configuration;

        public NotificacionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> EnviarNotificacionEmail(string destinatario, string asunto, string mensaje)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                var smtpServer = emailSettings["SmtpServer"];
                var smtpPort = int.Parse(emailSettings["SmtpPort"]);
                var smtpUsername = emailSettings["SmtpUsername"];
                var smtpPassword = emailSettings["SmtpPassword"];
                var emailFrom = emailSettings["EmailFrom"];

                var client = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailFrom, "Cooperativa Financiera"),
                    Subject = asunto,
                    Body = mensaje,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(destinatario);

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // En un entorno de producción, registrar el error en un sistema de logging
                Console.WriteLine($"Error al enviar notificación: {ex.Message}");
                return false;
            }
        }
    }
}