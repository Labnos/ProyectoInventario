using FluentValidation;
using ProyectoInventario.Models;

namespace ProyectoInventario.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Username)
            .NotEmpty().MaximumLength(50);

        RuleFor(u => u.PasswordHash)
            .NotEmpty();

        RuleFor(u => u.Role)
            .NotEmpty().MaximumLength(20);

        RuleFor(u => u.NombreCompleto)
            .MaximumLength(100);

        RuleFor(u => u.Correo)
            .EmailAddress().When(u => !string.IsNullOrWhiteSpace(u.Correo))
            .MaximumLength(100);

        RuleFor(u => u.Telefono)
            .MaximumLength(20);

        RuleFor(u => u.SucursalAsignada)
            .MaximumLength(50);
    }
}