using Domain.Interfaces.Generics;
using Domain.Interfaces.ICategoria;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio
{
    public class RepositorioCategoria : RepositoryGenerics<Categoria>, InterfaceCategoria, IInjectable
    {
        private readonly ContextBase _context;

        public RepositorioCategoria(ContextBase context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<Categoria>> ListarCategoriasUsuario(string emailUsuario)
        {
            return await
                (from s in _context.SistemaFinanceiro
                 join c in _context.Categoria on s.Id equals c.IdSistema
                 join us in _context.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                 where us.EmailUsuario.Equals(emailUsuario) && us.SistemaAtual
                 select c
                 ).AsNoTracking().ToListAsync();
        }
    }
}
