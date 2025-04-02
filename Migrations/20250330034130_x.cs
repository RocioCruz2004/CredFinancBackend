using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class x : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoCredito",
                columns: table => new
                {
                    idTipoCredito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCredito", x => x.idTipoCredito);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    idAdministrador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    cargo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    departamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nivelAcceso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrador", x => x.idAdministrador);
                    table.ForeignKey(
                        name: "FK_Administrador_Usuario_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    genero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    nacionalidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_Usuario_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    idAuditLog = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    usuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    detalles = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    idAdministrador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.idAuditLog);
                    table.ForeignKey(
                        name: "FK_AuditLog_Administrador_idAdministrador",
                        column: x => x.idAdministrador,
                        principalTable: "Administrador",
                        principalColumn: "idAdministrador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parametros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TasaInteres = table.Column<double>(type: "float", nullable: false),
                    PlazoMaximo = table.Column<int>(type: "int", nullable: false),
                    FormatoContrato = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdAdministrador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parametros_Administrador_IdAdministrador",
                        column: x => x.IdAdministrador,
                        principalTable: "Administrador",
                        principalColumn: "idAdministrador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Monto = table.Column<double>(type: "float", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportesFinancieros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPagado = table.Column<double>(type: "float", nullable: false),
                    TotalMorosidad = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportesFinancieros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportesFinancieros_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudCredito",
                columns: table => new
                {
                    idSolicitudCredito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    monto = table.Column<double>(type: "float", nullable: false),
                    ingresos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    idCliente = table.Column<int>(type: "int", nullable: false),
                    idTipoCredito = table.Column<int>(type: "int", nullable: false),
                    idParametro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudCredito", x => x.idSolicitudCredito);
                    table.ForeignKey(
                        name: "FK_SolicitudCredito_Clientes_idCliente",
                        column: x => x.idCliente,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudCredito_Parametros_idParametro",
                        column: x => x.idParametro,
                        principalTable: "Parametros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudCredito_TipoCredito_idTipoCredito",
                        column: x => x.idTipoCredito,
                        principalTable: "TipoCredito",
                        principalColumn: "idTipoCredito",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fechaFirma = table.Column<DateTime>(type: "datetime2", nullable: true),
                    contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    firmaDigital = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    metodoAutenticacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    codigoVerificacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    idSolicitudCredito = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Contratos_SolicitudCredito_idSolicitudCredito",
                        column: x => x.idSolicitudCredito,
                        principalTable: "SolicitudCredito",
                        principalColumn: "idSolicitudCredito",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreditoAprobado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaAprobacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontoAprobado = table.Column<double>(type: "float", nullable: false),
                    TasaAplicada = table.Column<double>(type: "float", nullable: false),
                    plazoAprobado = table.Column<int>(type: "int", nullable: false),
                    idSolicitudCredito = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditoAprobado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditoAprobado_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CreditoAprobado_SolicitudCredito_idSolicitudCredito",
                        column: x => x.idSolicitudCredito,
                        principalTable: "SolicitudCredito",
                        principalColumn: "idSolicitudCredito",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documento",
                columns: table => new
                {
                    idDocumento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fechaCarga = table.Column<DateTime>(type: "datetime2", nullable: false),
                    urlArchivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    idSolicitudCredito = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documento", x => x.idDocumento);
                    table.ForeignKey(
                        name: "FK_Documento_SolicitudCredito_idSolicitudCredito",
                        column: x => x.idSolicitudCredito,
                        principalTable: "SolicitudCredito",
                        principalColumn: "idSolicitudCredito",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvaluacionCredito",
                columns: table => new
                {
                    idEvaluacionCredito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    decision = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    comentarios = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    fechaEvaluacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idSolicitudCredito = table.Column<int>(type: "int", nullable: false),
                    idAdministrador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluacionCredito", x => x.idEvaluacionCredito);
                    table.ForeignKey(
                        name: "FK_EvaluacionCredito_Administrador_idAdministrador",
                        column: x => x.idAdministrador,
                        principalTable: "Administrador",
                        principalColumn: "idAdministrador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluacionCredito_SolicitudCredito_idSolicitudCredito",
                        column: x => x.idSolicitudCredito,
                        principalTable: "SolicitudCredito",
                        principalColumn: "idSolicitudCredito",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notificacion",
                columns: table => new
                {
                    idNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mensaje = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    fechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    idSolicitudCredito = table.Column<int>(type: "int", nullable: false),
                    idCreditoAprobado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacion", x => x.idNotificacion);
                    table.ForeignKey(
                        name: "FK_Notificacion_CreditoAprobado_idCreditoAprobado",
                        column: x => x.idCreditoAprobado,
                        principalTable: "CreditoAprobado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacion_SolicitudCredito_idSolicitudCredito",
                        column: x => x.idSolicitudCredito,
                        principalTable: "SolicitudCredito",
                        principalColumn: "idSolicitudCredito",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administrador_idUsuario",
                table: "Administrador",
                column: "idUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_idAdministrador",
                table: "AuditLog",
                column: "idAdministrador");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_idUsuario",
                table: "Clientes",
                column: "idUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_idSolicitudCredito",
                table: "Contratos",
                column: "idSolicitudCredito",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditoAprobado_ClienteId",
                table: "CreditoAprobado",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditoAprobado_idSolicitudCredito",
                table: "CreditoAprobado",
                column: "idSolicitudCredito",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documento_idSolicitudCredito",
                table: "Documento",
                column: "idSolicitudCredito");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionCredito_idAdministrador",
                table: "EvaluacionCredito",
                column: "idAdministrador");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionCredito_idSolicitudCredito",
                table: "EvaluacionCredito",
                column: "idSolicitudCredito");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_idCreditoAprobado",
                table: "Notificacion",
                column: "idCreditoAprobado");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_idSolicitudCredito",
                table: "Notificacion",
                column: "idSolicitudCredito");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ClienteId",
                table: "Pagos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Parametros_IdAdministrador",
                table: "Parametros",
                column: "IdAdministrador");

            migrationBuilder.CreateIndex(
                name: "IX_ReportesFinancieros_ClienteId",
                table: "ReportesFinancieros",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCredito_idCliente",
                table: "SolicitudCredito",
                column: "idCliente");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCredito_idParametro",
                table: "SolicitudCredito",
                column: "idParametro");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCredito_idTipoCredito",
                table: "SolicitudCredito",
                column: "idTipoCredito");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropTable(
                name: "Documento");

            migrationBuilder.DropTable(
                name: "EvaluacionCredito");

            migrationBuilder.DropTable(
                name: "Notificacion");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "ReportesFinancieros");

            migrationBuilder.DropTable(
                name: "CreditoAprobado");

            migrationBuilder.DropTable(
                name: "SolicitudCredito");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Parametros");

            migrationBuilder.DropTable(
                name: "TipoCredito");

            migrationBuilder.DropTable(
                name: "Administrador");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
