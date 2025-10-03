using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = "Encargado"; // Ej: Admin, Encargado, Vendedor

    [MaxLength(100)]
    public string NombreCompleto { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Correo { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Telefono { get; set;