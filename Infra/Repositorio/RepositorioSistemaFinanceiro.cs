using Domain.Interfaces.Generics;
using Domain.Interfaces.ISistemaFinanceiro;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio
{
    public class RepositorioSistemaFinanceiro : RepositoryGenerics<SistemaFinanceiro>, InterfaceSistemaFinanceiro, IInjectable
    {
        private readonly ContextBase _context;

        public RepositorioSistemaFinanceiro(ContextBase context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExecuteCopiaDespesasSistemaFinanceiro()
        {
            var listSistemaFinanceiro = new List<SistemaFinanceiro>();

            try
            {
                listSistemaFinanceiro = await _context.SistemaFinanceiro.Where(s => s.GerarCopiaDespesa).ToListAsync();

                listSistemaFinanceiro.ForEach(async s =>
                {
                    var dataAtual = DateTime.Now;
                    var mes = dataAtual.Month;
                    var ano = dataAtual.Year;
                    var despesaJaExiste = await (from d in _context.Despesa
                                                 join c in _context.Categoria on d.IdCategoria equals c.Id
                                                 where c.IdSistema == s.Id && d.Mes == mes && d.Ano == ano
                                                 select d.Id).AnyAsync();

                    if (!despesaJaExiste)
                    {
                        var despesaGravar = new List<Despesa>();
                        var despesasSistema = await (from d in _context.Despesa
                                                     join c in _context.Categoria on d.IdCategoria equals c.Id
                                                     where c.IdSistema == s.Id
                                                     && d.Mes == s.MesCopia
                                                     && d.Ano == s.AnoCopia
                                                     select d).ToListAsync();

                        despesasSistema.ForEach(d =>
                        {
                            var despesa = new Despesa()
                            {
                                Nome = d.Nome,
                                Valor = d.Valor,
                                Mes = mes,
                                Ano = ano,
                                TipoDespesa = d.TipoDespesa,
                                DataCadastro = dataAtual,
                                DataAlteracao = DateTime.MinValue,
                                DataPagamento = DateTime.MinValue,
                                DataVencimento = new DateTime(ano, mes, d.DataVencimento.Day),
                                Pago = false,
                                IdCategoria = d.IdCategoria,
                            };

                            despesaGravar.Add(despesa);
                        });

                        if (despesaGravar.Any())
                        {
                            _context.Despesa.AddRange(despesaGravar);
                            await _context.SaveChangesAsync();
                        }
                    }
                });
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<IList<SistemaFinanceiro>> ListarSistemasUsuario(string emailUsuario)
        {
            return await
                (from s in _context.SistemaFinanceiro
                 join us in _context.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                 where us.EmailUsuario.Equals(emailUsuario)
                 select s
                )
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
