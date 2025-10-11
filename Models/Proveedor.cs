using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Models;

public class Proveedor
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string NombreEmpresa { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string NombreContacto { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Correo { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Direccion { get; set; } = string.Empty;

    [MaxLength(50)]
    public string TipoProducto { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Observaciones { get; set; } = string.Empty;

    public string? NombreCompleto { get; set; } = string.Empty;
    // Si decides relacionar productos con proveedores:
    // public List<Producto> Productos { get; set; } = new();
}