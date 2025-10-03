using FluentValidation;
using ProyectoInventario.Models;

namespace ProyectoInventario.Validators;

public class ClienteValidator : AbstractValidator<Cliente>
{
    public ClienteValidator()
    {
        RuleFor(c => c.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100);

        RuleFor(c => c.Telefono)
            .MaximumLength(20);

        RuleFor(c => c.Direccion)
            .MaximumLength(200);

        RuleFor(c => c.Correo)
            .EmailAddress().When(c => !string.IsNullOrWhiteSpace(c.Correo))
            .MaximumLength(100);

        RuleFor(c => c.DocumentoIdentidad)
            .MaximumLength(50);
    }
}