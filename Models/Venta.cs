using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Models;

public class Venta
{
    public int Id { get; set; }

    [Required]
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Total { get; set; }

    [Required]
    public int ClienteId { get; set; }

    public Cliente Cliente { get; set; } = null!;

    [MaxLength(50)]
    public string Sucursal { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Estado { get; set; } = "Completada"; // Completada, Pendiente, Cancelada

    public List<VentaDetalle> Detalles { get; set; } = new();

    public Envio? Envio { get; set; }
}