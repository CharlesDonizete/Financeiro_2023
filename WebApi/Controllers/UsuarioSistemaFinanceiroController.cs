using Domain.Interfaces.InterfaceServicos;
using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioSistemaFinanceiroController : ControllerBase
    {
        private readonly InterfaceUsuarioSistemaFinanceiro _interfaceUsuarioSistemaFinanceiro;
        private readonly IUsuarioSistemaFinanceiroServico _iUsuarioSistemaFinanceiroServico;

        public UsuarioSistemaFinanceiroController(
            InterfaceUsuarioSistemaFinanceiro interfaceUsuarioSistemaFinanceiro, 
            IUsuarioSistemaFinanceiroServico iUsuarioSistemaFinanceiroServico)
        {
            _interfaceUsuarioSistemaFinanceiro = interfaceUsuarioSistemaFinanceiro;
            _iUsuarioSistemaFinanceiroServico = iUsuarioSistemaFinanceiroServico;
        }

        [HttpGet("/api/ListarUsuariosSistema")]
        [Produces("application/json")]
        public async Task<object> ListarUsuariosSistema(int idSistema)
            => await _interfaceUsuarioSistemaFinanceiro.ListarUsuariosSistema(idSistema);

        [HttpPost("/api/CadastraUsuarioNoSistema")]
        [Produces("application/json")]
        public async Task<object> CadastraUsuarioNoSistema(int idSistema, string emailUsuario)
        {
            try
            {
                await _iUsuarioSistemaFinanceiroServico.CadastraUsuarioNoSistema(
                    new UsuarioSistemaFinanceiro
                    {
                        IdSistema = idSistema,
                        EmailUsuario = emailUsuario,
                        Administrador = false,
                        SistemaAtual = true,
                    });
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        [HttpDelete("/api/DeleteUsuarioSistemaFinanceiro")]
        [Produces("application/json")]
        public async Task<object> DeleteUsuarioSistemaFinanceiro(int id)
        {
            try
            {
                var usuarioSistemaFinanceiro = await _interfaceUsuarioSistemaFinanceiro.GetEntityById(id);

                await _interfaceUsuarioSistemaFinanceiro.Delete(usuarioSistemaFinanceiro);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
