using FluentValidation;
using WebApi.Models;

namespace WebApi.Validators
{
    public class CategoriaValidator : AbstractValidator<CategoriaModel>
    {
        public CategoriaValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome da categoria é obrigatório!");
            RuleFor(x => x.IdSistema).NotEmpty().WithMessage("O código do sistema é obrigatório!");
        }
    }
}
