// En: Validators/UserLoginDtoValidator.cs
using FluentValidation;
using ProyectoInventario.Endpoints; // Namespace donde está tu clase UserLoginDto

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.");
    }
}