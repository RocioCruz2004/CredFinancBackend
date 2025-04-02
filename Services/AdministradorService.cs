using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Data;
using proyectoFinal.Modelo;

namespace CooperativaFinanciera.Services
{
    public interface IAdministradorService
    {
        Task<Administrador> GetAdministradorAsync(int idAdministrador);
        Task<string> GetAdministradorNombreAsync(int idAdministrador);
    }

    public class AdministradorService : IAdministradorService
    {
        private readonly DBCconexion dBConexion;

        public AdministradorService(DBCconexion context)
        {
            dBConexion = context;
        }

        public async Task<Administrador> GetAdministradorAsync(int idAdministrador)
        {
            return await dBConexion.Administrador
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.idAdministrador == idAdministrador);
        }

        public async Task<string> GetAdministradorNombreAsync(int idAdministrador)
        {
            var admin = await dBConexion.Administrador
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.idAdministrador == idAdministrador);

            return admin?.Usuario?.nombre ?? $"Administrador ID: {idAdministrador}";
        }
    }
}