using Microsoft.EntityFrameworkCore;
using proyectoFinal.Modelo;

namespace proyectoFinal.Data
{
    public class DBCconexion : DbContext
    {
        public DBCconexion(DbContextOptions<DBCconexion> options)
            : base(options)
        {
        }

        // DbSets para las entidades que van a ser mapeadas en la base de datos
        public DbSet<Administrador> Administrador { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CreditoAprobado> CreditoAprobado { get; set; }
        public DbSet<Documento> Documento { get; set; }
        public DbSet<EvaluacionCredito> EvaluacionCredito{ get; set; }
        public DbSet<Notificacion> Notificacion { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<ReporteFinanciero> ReportesFinancieros { get; set; }
        public DbSet<SolicitudCredito> SolicitudCredito { get; set; }
        public DbSet<TipoCredito> TipoCredito { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Contrato> Contratos { get; set; }

        // Métodos adicionales para la configuración de la base de datos, si es necesario
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Usuario - Administrador (1:1)
            modelBuilder.Entity<Administrador>()
                .HasOne(a => a.Usuario)
                .WithOne(u => u.Administrador)
                .HasForeignKey<Administrador>(a => a.idUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario - Cliente (1:1)
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithOne(u => u.Cliente)
                .HasForeignKey<Cliente>(c => c.idUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Cliente - SolicitudCredito (1:N)
            modelBuilder.Entity<SolicitudCredito>()
                .HasOne(s => s.Cliente)
                .WithMany(c => c.SolicitudesCredito)
                .HasForeignKey(s => s.idCliente)
                .OnDelete(DeleteBehavior.Restrict);

            // TipoCredito - SolicitudCredito (1:N)
            modelBuilder.Entity<SolicitudCredito>()
                .HasOne(s => s.TipoCredito)
                .WithMany()
                .HasForeignKey(s => s.idTipoCredito)
                .OnDelete(DeleteBehavior.Restrict);

            // Parametro - SolicitudCredito (1:N)
            modelBuilder.Entity<SolicitudCredito>()
                .HasOne(s => s.Parametro)
                .WithMany()
                .HasForeignKey(s => s.idParametro)
                .OnDelete(DeleteBehavior.Restrict);

            // SolicitudCredito - Documento (1:N)
            modelBuilder.Entity<Documento>()
                .HasOne(d => d.SolicitudCredito)
                .WithMany(s => s.Documentos)
                .HasForeignKey(d => d.idSolicitudCredito)
                .OnDelete(DeleteBehavior.Cascade);

            // SolicitudCredito - EvaluacionCredito (1:N)
            modelBuilder.Entity<EvaluacionCredito>()
                .HasOne(e => e.SolicitudCredito)
                .WithMany(s => s.Evaluaciones)
                .HasForeignKey(e => e.idSolicitudCredito)
                .OnDelete(DeleteBehavior.Restrict);

            // Administrador - EvaluacionCredito (1:N)
            modelBuilder.Entity<EvaluacionCredito>()
                .HasOne(e => e.Administrador)
                .WithMany(a => a.Evaluaciones)
                .HasForeignKey(e => e.idAdministrador)
                .OnDelete(DeleteBehavior.Restrict);

            // SolicitudCredito - CreditoAprobado (1:1)
            modelBuilder.Entity<CreditoAprobado>()
                .HasOne(c => c.SolicitudCredito)
                .WithOne(s => s.CreditoAprobado)
                .HasForeignKey<CreditoAprobado>(c => c.idSolicitudCredito)
                .OnDelete(DeleteBehavior.Restrict);

            // SolicitudCredito - Notificacion (1:N)
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.SolicitudCredito)
                .WithMany(s => s.Notificaciones)
                .HasForeignKey(n => n.idSolicitudCredito)
                .OnDelete(DeleteBehavior.Restrict);

            // CreditoAprobado - Notificacion (1:N)
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.CreditoAprobado)
                .WithMany(c => c.Notificaciones)
                .HasForeignKey(n => n.idCreditoAprobado)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Administrador - AuditLog (1:N)
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.Administrador)
                .WithMany(a => a.AuditLogs)
                .HasForeignKey(a => a.idAdministrador)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Contrato>()
                .HasOne(c => c.SolicitudCredito)
                .WithOne()
                .HasForeignKey<Contrato>(c => c.idSolicitudCredito)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

