using Domain.Interfaces.InterfaceServicos;
using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

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
    public async Task<IList<UsuarioSistemaFinanceiro>> ListarUsuariosSistema(int idSistema)
        => await _interfaceUsuarioSistemaFinanceiro.ListarUsuariosSistema(idSistema);

    [HttpPost("/api/CadastraUsuarioNoSistema")]
    [Produces("application/json")]
    public async Task<bool> CadastraUsuarioNoSistema(int idSistema, string emailUsuario)
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
            return false;
        }

        return true;
    }

    [HttpDelete("/api/DeleteUsuarioSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<bool> DeleteUsuarioSistemaFinanceiro(int id)
    {
        try
        {
            var usuarioSistemaFinanceiro = await _interfaceUsuarioSistemaFinanceiro.GetEntityById(id);

            await _interfaceUsuarioSistemaFinanceiro.Delete(usuarioSistemaFinanceiro);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}