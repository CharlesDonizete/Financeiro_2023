using Domain.Interfaces.ISistemaFinanceiro;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio
{
    public class RepositorioSistemaFinanceiro : RepositoryGenerics<SistemaFinanceiro>, InterfaceSistemaFinanceiro
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public RepositorioSistemaFinanceiro()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<bool> ExecuteCopiaDespesasSistemaFinanceiro()
        {
            var listSistemaFinanceiro = new List<SistemaFinanceiro>();

            try
            {
                using (var banco = new ContextBase(_OptionsBuilder))
                {
                    listSistemaFinanceiro = await banco.SistemaFinanceiro.Where(s => s.GerarCopiaDespesa).ToListAsync();
                }

                listSistemaFinanceiro.ForEach(async s =>
                {
                    var dataAtual = DateTime.Now;
                    var mes = dataAtual.Month;
                    var ano = dataAtual.Year;
                    using (var banco = new ContextBase(_OptionsBuilder))
                    {
                        var despesaJaExiste = await (from d in banco.Despesa
                                                     join c in banco.Categoria on d.IdCategoria equals c.Id
                                                     where c.IdSistema == s.Id && d.Mes == mes && d.Ano == ano
                                                     select d.Id).AnyAsync();

                        if (!despesaJaExiste)
                        {
                            var despesaGravar = new List<Despesa>();
                            var despesasSistema = await (from d in banco.Despesa
                                                         join c in banco.Categoria on d.IdCategoria equals c.Id
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
                                banco.Despesa.AddRange(despesaGravar);
                                await banco.SaveChangesAsync();
                            }
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
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                return await
                    (from s in banco.SistemaFinanceiro
                     join us in banco.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                     where us.EmailUsuario.Equals(emailUsuario)
                     select s
                    )
                    .AsNoTracking()
                    .ToListAsync();
            }
        }
    }
}
