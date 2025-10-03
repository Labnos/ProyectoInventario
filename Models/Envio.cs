using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Models;

public class Envio
{
    public int Id { get; set; }

    [Required]
    public int VentaId { get; set; }

    public Venta Venta { get; set; } = null!;

    [Required]
    [MaxLength(30)]
    public string Estado { get; set; } = "Pendiente"; // Pendiente, En tránsito, Entregado

    [Required]
    [MaxLength(50)]
    public string Medio { get; set; } = string.Empty; // Mensajería, Entrega propia, etc.

    [MaxLength(100)]
    public string Guia { get; set; } = string.Empty;

    public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string Destinatario { get; set; } = string.Empty;

    [MaxLength(200)]
    public string DireccionEntrega { get; set; } = string.Empty;

    [MaxLength(20)]
    public string TelefonoContacto { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Observaciones { get; set; } = string.Empty;
}