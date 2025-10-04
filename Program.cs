using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FluentValidation;
using ProyectoInventario.Data;
using ProyectoInventario.Services;
using ProyectoInventario.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// --- REGISTRO DE SERVICIOS ---

// Configuración de la Base de Datos MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34))));

// Configuración de Autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Configuración de Autorización
builder.Services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// --- SOLUCIÓN AL ERROR: Registrar todos los validadores con una sola línea ---
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Registrar los servicios de la aplicación
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<VentaService>();
builder.Services.AddScoped<SucursalService>();
builder.Services.AddScoped<PromocionService>();
builder.Services.AddScoped<ReportService>();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Inventario", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configuración de CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


// --- CONSTRUCCIÓN DE LA APLICACIÓN ---
var app = builder.Build();

// Configuración del Pipeline de HTTP
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// --- MAPEO DE ENDPOINTS ---
app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapProductoEndpoints();
app.MapVentaEndpoints();
app.MapClienteEndpoints();
app.MapProveedorEndpoints();
app.MapSucursalEndpoints();
app.MapPromocionEndpoints();
app.MapReportEndpoints();
// app.MapEnvioEndpoints(); // Comentado si no está implementado

app.MapGet("/", () => "API del Proyecto de Inventario está en ejecución.");

// --- EJECUTAR LA APLICACIÓN ---
xapp.Run();