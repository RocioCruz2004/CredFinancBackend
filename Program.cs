using CooperativaFinanciera.Services;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configura el DbContext con la cadena de conexión desde appsettings.json
builder.Services.AddDbContext<DBCconexion>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));  // Asegúrate de que la cadena de conexión sea correcta

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder
            .WithOrigins("http://localhost:3000") // Permite solicitudes desde el frontend en localhost:3000
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Agregar servicios de controladores con configuración de JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Registrar servicios
builder.Services.AddScoped<INotificacionService, CooperativaFinanciera.Services.NotificacionService>();

// Configuración para Swagger/OpenAPI (opcional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del pipeline HTTP de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors("AllowLocalhost");

app.UseAuthorization();

// Mapea las rutas de los controladores
app.MapControllers();

app.Run();
