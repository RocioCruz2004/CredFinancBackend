﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using proyectoFinal.Data;

#nullable disable

namespace proyectoFinal.Migrations
{
    [DbContext(typeof(DBCconexion))]
    partial class DBCconexionModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("proyectoFinal.Modelo.Administrador", b =>
                {
                    b.Property<int>("idAdministrador")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idAdministrador"));

                    b.Property<string>("cargo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("departamento")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.Property<int>("nivelAcceso")
                        .HasColumnType("int");

                    b.Property<string>("rol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("idAdministrador");

                    b.HasIndex("idUsuario")
                        .IsUnique();

                    b.ToTable("Administrador");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.AuditLog", b =>
                {
                    b.Property<int>("idAuditLog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idAuditLog"));

                    b.Property<string>("accion")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("detalles")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("idAdministrador")
                        .HasColumnType("int");

                    b.Property<string>("usuario")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("idAuditLog");

                    b.HasIndex("idAdministrador");

                    b.ToTable("AuditLog");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("direccion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("fechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("genero")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("idUsuario")
                        .HasColumnType("int");

                    b.Property<string>("nacionalidad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("idUsuario")
                        .IsUnique();

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Contrato", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("codigoVerificacion")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("contenido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("estado")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("fechaFirma")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("fechaGeneracion")
                        .HasColumnType("datetime2");

                    b.Property<string>("firmaDigital")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("idSolicitudCredito")
                        .HasColumnType("int");

                    b.Property<string>("metodoAutenticacion")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("id");

                    b.HasIndex("idSolicitudCredito")
                        .IsUnique();

                    b.ToTable("Contratos");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.CreditoAprobado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClienteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaAprobacion")
                        .HasColumnType("datetime2");

                    b.Property<double>("MontoAprobado")
                        .HasColumnType("float");

                    b.Property<double>("TasaAplicada")
                        .HasColumnType("float");

                    b.Property<int>("idSolicitudCredito")
                        .HasColumnType("int");

                    b.Property<int>("plazoAprobado")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("idSolicitudCredito")
                        .IsUnique();

                    b.ToTable("CreditoAprobado");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Documento", b =>
                {
                    b.Property<int>("idDocumento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idDocumento"));

                    b.Property<DateTime>("fechaCarga")
                        .HasColumnType("datetime2");

                    b.Property<int>("idSolicitudCredito")
                        .HasColumnType("int");

                    b.Property<string>("tipo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("urlArchivo")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("idDocumento");

                    b.HasIndex("idSolicitudCredito");

                    b.ToTable("Documento");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.EvaluacionCredito", b =>
                {
                    b.Property<int>("idEvaluacionCredito")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idEvaluacionCredito"));

                    b.Property<string>("comentarios")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("decision")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("fechaEvaluacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("idAdministrador")
                        .HasColumnType("int");

                    b.Property<int>("idSolicitudCredito")
                        .HasColumnType("int");

                    b.HasKey("idEvaluacionCredito");

                    b.HasIndex("idAdministrador");

                    b.HasIndex("idSolicitudCredito");

                    b.ToTable("EvaluacionCredito");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Notificacion", b =>
                {
                    b.Property<int>("idNotificacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idNotificacion"));

                    b.Property<DateTime>("fechaEnvio")
                        .HasColumnType("datetime2");

                    b.Property<int?>("idCreditoAprobado")
                        .HasColumnType("int");

                    b.Property<int>("idSolicitudCredito")
                        .HasColumnType("int");

                    b.Property<string>("mensaje")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("tipo")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("idNotificacion");

                    b.HasIndex("idCreditoAprobado");

                    b.HasIndex("idSolicitudCredito");

                    b.ToTable("Notificacion");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Pago", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaPago")
                        .HasColumnType("datetime2");

                    b.Property<double>("Monto")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Pagos");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Parametro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FormatoContrato")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("IdAdministrador")
                        .HasColumnType("int");

                    b.Property<int>("PlazoMaximo")
                        .HasColumnType("int");

                    b.Property<double>("TasaInteres")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("IdAdministrador");

                    b.ToTable("Parametros");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.ReporteFinanciero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaGeneracion")
                        .HasColumnType("datetime2");

                    b.Property<double>("TotalMorosidad")
                        .HasColumnType("float");

                    b.Property<double>("TotalPagado")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("ReportesFinancieros");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.SolicitudCredito", b =>
                {
                    b.Property<int>("idSolicitudCredito")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idSolicitudCredito"));

                    b.Property<string>("estado")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("fechaSolicitud")
                        .HasColumnType("datetime2");

                    b.Property<int>("idCliente")
                        .HasColumnType("int");

                    b.Property<int>("idParametro")
                        .HasColumnType("int");

                    b.Property<int>("idTipoCredito")
                        .HasColumnType("int");

                    b.Property<decimal>("ingresos")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("monto")
                        .HasColumnType("float");

                    b.Property<string>("observaciones")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("idSolicitudCredito");

                    b.HasIndex("idCliente");

                    b.HasIndex("idParametro");

                    b.HasIndex("idTipoCredito");

                    b.ToTable("SolicitudCredito");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.TipoCredito", b =>
                {
                    b.Property<int>("idTipoCredito")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idTipoCredito"));

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("idTipoCredito");

                    b.ToTable("TipoCredito");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Usuario", b =>
                {
                    b.Property<int>("idUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idUsuario"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("idUsuario");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Administrador", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.Usuario", "Usuario")
                        .WithOne("Administrador")
                        .HasForeignKey("proyectoFinal.Modelo.Administrador", "idUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.AuditLog", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.Administrador", "Administrador")
                        .WithMany("AuditLogs")
                        .HasForeignKey("idAdministrador")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Administrador");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Cliente", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.Usuario", "Usuario")
                        .WithOne("Cliente")
                        .HasForeignKey("proyectoFinal.Modelo.Cliente", "idUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Contrato", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.SolicitudCredito", "SolicitudCredito")
                        .WithOne()
                        .HasForeignKey("proyectoFinal.Modelo.Contrato", "idSolicitudCredito")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SolicitudCredito");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.CreditoAprobado", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.Cliente", null)
                        .WithMany("CreditosAprobados")
                        .HasForeignKey("ClienteId");

                    b.HasOne("proyectoFinal.Modelo.SolicitudCredito", "SolicitudCredito")
                        .WithOne("CreditoAprobado")
                        .HasForeignKey("proyectoFinal.Modelo.CreditoAprobado", "idSolicitudCredito")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SolicitudCredito");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Documento", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.SolicitudCredito", "SolicitudCredito")
                        .WithMany("Documentos")
                        .HasForeignKey("idSolicitudCredito")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SolicitudCredito");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.EvaluacionCredito", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.Administrador", "Administrador")
                        .WithMany("Evaluaciones")
                        .HasForeignKey("idAdministrador")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("proyectoFinal.Modelo.SolicitudCredito", "SolicitudCredito")
                        .WithMany("Evaluaciones")
                        .HasForeignKey("idSolicitudCredito")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Administrador");

                    b.Navigation("SolicitudCredito");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Notificacion", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.CreditoAprobado", "CreditoAprobado")
                        .WithMany("Notificaciones")
                        .HasForeignKey("idCreditoAprobado")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("proyectoFinal.Modelo.SolicitudCredito", "SolicitudCredito")
                        .WithMany("Notificaciones")
                        .HasForeignKey("idSolicitudCredito")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreditoAprobado");

                    b.Navigation("SolicitudCredito");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Pago", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.Cliente", "Cliente")
                        .WithMany("Pagos")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Parametro", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.Administrador", "administrador")
                        .WithMany()
                        .HasForeignKey("IdAdministrador")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("administrador");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.ReporteFinanciero", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.Cliente", "Cliente")
                        .WithMany("ReportesFinancieros")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.SolicitudCredito", b =>
                {
                    b.HasOne("proyectoFinal.Modelo.Cliente", "Cliente")
                        .WithMany("SolicitudesCredito")
                        .HasForeignKey("idCliente")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("proyectoFinal.Modelo.Parametro", "Parametro")
                        .WithMany()
                        .HasForeignKey("idParametro")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("proyectoFinal.Modelo.TipoCredito", "TipoCredito")
                        .WithMany()
                        .HasForeignKey("idTipoCredito")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Parametro");

                    b.Navigation("TipoCredito");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Administrador", b =>
                {
                    b.Navigation("AuditLogs");

                    b.Navigation("Evaluaciones");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Cliente", b =>
                {
                    b.Navigation("CreditosAprobados");

                    b.Navigation("Pagos");

                    b.Navigation("ReportesFinancieros");

                    b.Navigation("SolicitudesCredito");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.CreditoAprobado", b =>
                {
                    b.Navigation("Notificaciones");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.SolicitudCredito", b =>
                {
                    b.Navigation("CreditoAprobado");

                    b.Navigation("Documentos");

                    b.Navigation("Evaluaciones");

                    b.Navigation("Notificaciones");
                });

            modelBuilder.Entity("proyectoFinal.Modelo.Usuario", b =>
                {
                    b.Navigation("Administrador");

                    b.Navigation("Cliente");
                });
#pragma warning restore 612, 618
        }
    }
}
