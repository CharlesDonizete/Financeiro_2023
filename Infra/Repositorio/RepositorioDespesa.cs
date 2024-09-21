using Domain.Interfaces.Generics;
using Domain.Interfaces.IDespesa;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio
{
    public class RepositorioDespesa : RepositoryGenerics<Despesa>, InterfaceDespesa, IInjectable
    {
        private readonly ContextBase _context;

        public RepositorioDespesa(ContextBase context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<Despesa>> ListarDespesasUsuario(string emailUsuario)
        {
            return await
                (from s in _context.SistemaFinanceiro
                 join c in _context.Categoria on s.Id equals c.IdSistema
                 join us in _context.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                 join d in _context.Despesa on c.Id equals d.IdCategoria
                 where us.EmailUsuario.Equals(emailUsuario) &&
                 s.Mes == d.Mes &&
                 s.Ano == d.Ano
                 select d
                 ).AsNoTracking().ToListAsync();
        }

        public async Task<IList<Despesa>> ListarDespesasUsuarioNaoPagasMesesAnteriores(string emailUsuario)
        {
            return await
                (from s in _context.SistemaFinanceiro
                 join c in _context.Categoria on s.Id equals c.IdSistema
                 join us in _context.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                 join d in _context.Despesa on c.Id equals d.IdCategoria
                 where us.EmailUsuario.Equals(emailUsuario) &&
                 d.Mes < DateTime.Now.Month &&
                 !d.Pago
                 select d
                 ).AsNoTracking().ToListAsync();
        }
    }
}
