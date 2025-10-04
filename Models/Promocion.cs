using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Models;

public class Promocion
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    [Required]
    public decimal PorcentajeDescuento { get; set; }

    [Required]
    public DateTime FechaInicio { get; set; }

    [Required]
    public DateTime FechaFin { get; set; }

    public bool Activa { get; set; } = true;
}