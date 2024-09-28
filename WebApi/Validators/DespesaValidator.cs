using Domain.Interfaces.InterfaceServicos;
using FluentValidation;
using WebApi.Models;

namespace WebApi.Validators
{
    public class DespesaValidator : AbstractValidator<DespesaModel>
    {
        private readonly ICategoriaServico _categoriaServico;

        public DespesaValidator(ICategoriaServico categoriaServico)
        {
            _categoriaServico = categoriaServico;

            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome da Despesa é obrigatório!");
            
            RuleFor(x => x.IdCategoria)
                .Must(idSistema => _categoriaServico.Existe(idSistema).Result)
                .WithMessage("A categoria com ID {PropertyValue} não existe.");
        }
    }
}
