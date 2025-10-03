using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Models;

public class Sucursal
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Direccion { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Correo { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Ciudad { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Departamento { get; set; } = string.Empty;

    public bool Activa { get; set; } = true;

    // Si decides relacionar productos o ventas con sucursal:
    // public List<Producto> Productos { get; set; } = new();
    // public List<Venta> Ventas { get; set; } = new();
}