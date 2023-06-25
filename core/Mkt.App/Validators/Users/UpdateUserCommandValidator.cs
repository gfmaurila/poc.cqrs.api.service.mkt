using FluentValidation;
using Mkt.App.Commands.Users.UpdateUser;
using System.Text.RegularExpressions;

namespace Mkt.App.Validators.Users;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(p => p.Email)
            .EmailAddress()
            .WithMessage("E-mail não válido!");

        RuleFor(p => p.Password)
            .Must(ValidPassword)
            .WithMessage("Senha deve conter pelo menos 8 caracteres, um número, uma letra maiúscula, uma minúscula e um caractere especial");

        RuleFor(p => p.FullName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Nome é obrigatório!");

        RuleFor(p => p.Phone)
            .NotNull()
            .NotEmpty()
            .WithMessage("Celular é obrigatório!");
    }

    public bool ValidPassword(string password)
    {
        var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");

        return regex.IsMatch(password);
    }

}
