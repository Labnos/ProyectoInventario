using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Models;

public class VentaDetalle
{
    public int Id { get; set; }

    [Required]
    public int VentaId { get; set; }

    public Venta Venta { get; set; } = null!;

    [Required]
    public int ProductoId { get; set; }

    public Producto Producto { get; set; } = null!;

    [Required]
    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal PrecioUnitario { get; set; }

    // Subtotal calculado automáticamente
    public decimal Subtotal => PrecioUnitario * Cantidad;
}