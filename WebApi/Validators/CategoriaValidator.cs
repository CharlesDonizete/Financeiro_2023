using Domain.Interfaces.InterfaceServicos;
using FluentValidation;
using WebApi.Models;

namespace WebApi.Validators
{
    public class CategoriaValidator : AbstractValidator<CategoriaModel>
    {
        private readonly ISistemaFinanceiroServico _sistemaFinanceiroServico;

        public CategoriaValidator(ISistemaFinanceiroServico sistemaFinanceiroServico)
        {
            _sistemaFinanceiroServico = sistemaFinanceiroServico;

            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome da categoria é obrigatório!");
            RuleFor(x => x.IdSistema).NotEmpty().WithMessage("O código do sistema é obrigatório!");

            RuleFor(x => x.IdSistema)
                .Must(idSistema => _sistemaFinanceiroServico.Existe(idSistema).Result)
                .WithMessage("O Sistema Financeiro com ID {PropertyValue} não existe.");
        }
    }
}
