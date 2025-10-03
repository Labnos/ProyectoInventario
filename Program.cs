using ProyectoInventario.Data;
using ProyectoInventario.Models;
using ProyectoInventario.Auth;
using ProyectoInventario.Endpoints;
using ProyectoInventario.Services;
using ProyectoInventario.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración de base de datos MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 34))));

// Configuración de autenticación JWT
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

builder.Services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Validaciones con FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<ProductoValidator>();
builder.Services.AddScoped<IValidator<Cliente>, ClienteValidator>();
builder.Services.AddScoped<IValidator<Envio>, EnvioValidator>();
builder.Services.AddScoped<IValidator<Venta>, VentaValidator>();
builder.Services.AddScoped<IValidator<VentaDetalle>, VentaDetalleValidator>();
builder.Services.AddScoped<IValidator<User>, UserValidator>();
builder.Services.AddScoped<IValidator<Sucursal>, SucursalValidator>();
builder.Services.AddScoped<IValidator<Proveedor>, ProveedorValidator>();

// Servicios personalizados
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<Users>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<SucursalService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<VentaService>();

// Swagger con soporte JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT",
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

// CORS para frontend externo
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

//  Middleware
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints Minimal API
app.MapProductoEndpoints();
app.MapVentaEndpoints();
app.MapEnvioEndpoints();
app.MapAuthEndpoints();
app.MapClienteEndpoints();
app.MapSucursalEndpoints();
app.MapProveedorEndpoints();
app.MapUserEndpoints();
// Puedes agregar: app.MapReportEndpoints(); si tienes lógica de reportes

// Endpoint raíz
app.MapGet("/", () => "API ProyectoInventario corriendo en .NET 9");

// Ejecutar aplicación
app.Run();