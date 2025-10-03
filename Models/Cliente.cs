namespace ProyectoInventario.Models;

public class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string DocumentoIdentidad { get; set; } = string.Empty;

    // Relación con ventas
    public List<Venta> Ventas { get; set; } = new();
}