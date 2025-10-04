using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Models;

public class Producto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Talla { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Color { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal PrecioEntrada { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal PrecioVendedor { get; set; }

    [Required]
    [MaxLength(50)]
    public string Sucursal { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; } = 0;
    public object TipoPrenda { get; internal set; }
    public object Proveedor { get; internal set; }
    public object PrecioAdquisicion { get; internal set; }
    public object PrecioVenta { get; internal set; }
}
