using Domain.Interfaces.IDespesa;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DespesaController : ControllerBase
{
    private readonly InterfaceDespesa _interfaceDespesa;
    private readonly IDespesaServico _despesaServico;

    public DespesaController(
        InterfaceDespesa interfaceDespesa,
        IDespesaServico DespesaServico)
    {
        _interfaceDespesa = interfaceDespesa;
        _despesaServico = DespesaServico;
    }

    [HttpGet("/api/ListarDespesasUsuario")]
    [Produces("application/json")]
    public async Task<IList<Despesa>> ListarDespesasUsuario(string emailUsuario)
        => await _interfaceDespesa.ListarDespesasUsuario(emailUsuario);

    [HttpPost("/api/AdicionarDespesa")]
    [Produces("application/json")]
    public async Task<Despesa> AdicionarDespesa(Despesa Despesa)
    {
        await _despesaServico.AdicionarDespesa(Despesa);

        return Despesa;
    }

    [HttpPut("/api/AtualizarDespesa")]
    [Produces("application/json")]
    public async Task<Despesa> AtualizarDespesa(Despesa Despesa)
    {
        await _despesaServico.AtualizarDespesa(Despesa);

        return Despesa;
    }

    [HttpGet("/api/ObterDespesa")]
    [Produces("application/json")]
    public async Task<Despesa> ObterDespesa(int id)
    {
        return await _interfaceDespesa.GetEntityById(id);
    }

    [HttpDelete("/api/DeletarDespesa")]
    [Produces("application/json")]
    public async Task<bool> DeletarDespesa(int id)
    {
        try
        {
            var Despesa = await _interfaceDespesa.GetEntityById(id);

            await _interfaceDespesa.Delete(Despesa);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    [HttpGet("/api/CarregaGraficos")]
    [Produces("application/json")]
    public async Task<object> CarregaGraficos(string emailUsuario)
        => await _despesaServico.CarregaGraficos(emailUsuario);
}