using Domain.Interfaces.InterfaceServicos;
using Domain.Interfaces.ISistemaFinanceiro;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SistemaFinanceiroController : ControllerBase
{
    private readonly InterfaceSistemaFinanceiro _interfaceSistemaFinanceiro;
    private readonly ISistemaFinanceiroServico _iSistemaFinanceiroServico;
    public SistemaFinanceiroController(
        InterfaceSistemaFinanceiro interfaceSistemaFinanceiro,
        ISistemaFinanceiroServico iSistemaFinanceiroServico)
    {
        _interfaceSistemaFinanceiro = interfaceSistemaFinanceiro;
        _iSistemaFinanceiroServico = iSistemaFinanceiroServico;
    }

    [HttpGet("/api/ListaSistemaUsuario")]
    [Produces("application/json")]
    public async Task<object> ListaSistemaUsuario(string emailUsuario)
        => await _interfaceSistemaFinanceiro.ListarSistemasUsuario(emailUsuario);

    [HttpPost("/api/AdicionarSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<SistemaFinanceiro> AdicionarSistemaFinanceiro(SistemaFinanceiro sistemaFinanceiro)
    {
        await _iSistemaFinanceiroServico.AdicionarSistemaFinanceiro(sistemaFinanceiro);

        return sistemaFinanceiro;
    }

    [HttpPut("/api/AtualizarSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<SistemaFinanceiro> AtualizarSistemaFinanceiro(SistemaFinanceiro sistemaFinanceiro)
    {
        await _iSistemaFinanceiroServico.AtualizarSistemaFinanceiro(sistemaFinanceiro);

        return sistemaFinanceiro;
    }

    [HttpGet("/api/ObterSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<SistemaFinanceiro> ObterSistemaFinanceiro(int id)
        => await _interfaceSistemaFinanceiro.GetEntityById(id);

    [HttpDelete("/api/DeleteSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<bool> DeleteSistemaFinanceiro(int id)
    {
        try
        {
            var sistemaFinanceiro = await _interfaceSistemaFinanceiro.GetEntityById(id);
            await _interfaceSistemaFinanceiro.Delete(sistemaFinanceiro);
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }
}