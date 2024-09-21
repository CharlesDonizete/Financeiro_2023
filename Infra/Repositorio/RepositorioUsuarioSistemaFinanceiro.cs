using Domain.Interfaces.Generics;
using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio
{
    public class RepositorioUsuarioSistemaFinanceiro : RepositoryGenerics<UsuarioSistemaFinanceiro>, InterfaceUsuarioSistemaFinanceiro, IInjectable
    {
        private readonly ContextBase _context;

        public RepositorioUsuarioSistemaFinanceiro(ContextBase context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<UsuarioSistemaFinanceiro>> ListarUsuariosSistema(int IdSistema)
        {
            return await
                _context.UsuarioSistemaFinanceiro
                .Where(s => s.IdSistema == IdSistema)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UsuarioSistemaFinanceiro?> ObterUsuarioPorEmail(string emailUsuario)
        {
            return await
                _context.UsuarioSistemaFinanceiro
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EmailUsuario.Equals(emailUsuario));
        }

        public async Task RemoverUsuarios(List<UsuarioSistemaFinanceiro> usuarios)
        {
            _context.UsuarioSistemaFinanceiro
            .RemoveRange(usuarios);

            await _context.SaveChangesAsync();
        }
    }
}
