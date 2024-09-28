using AutoMapper;
using Domain.Interfaces.IDespesa;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Models;

namespace WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DespesaController : ControllerBase
{
    private readonly InterfaceDespesa _interfaceDespesa;
    private readonly IDespesaServico _despesaServico;
    private readonly IMapper _mapper;

    public DespesaController(
        InterfaceDespesa interfaceDespesa,
        IDespesaServico DespesaServico,
        IMapper mapper)
    {
        _interfaceDespesa = interfaceDespesa;
        _despesaServico = DespesaServico;
        _mapper = mapper;
    }

    [HttpGet("/api/ListarDespesasUsuario")]
    [Produces("application/json")]
    public async Task<IActionResult> ListarDespesasUsuario(string emailUsuario)
    {
        var listaDespesas = await _interfaceDespesa.ListarDespesasUsuario(emailUsuario);

        return Ok(_mapper.Map<IList<DespesaModel>>(listaDespesas));
    }

    [HttpPost("/api/AdicionarDespesa")]
    [Produces("application/json")]
    public async Task<IActionResult> AdicionarDespesa(DespesaModel despesaModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErroModel(HttpContext));
        }

        var despesa = _mapper.Map<Despesa>(despesaModel);

        await _despesaServico.AdicionarDespesa(despesa);

        return Ok(_mapper.Map<DespesaModel>(despesaModel));
    }

    [HttpPut("/api/AtualizarDespesa")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarDespesa(DespesaModel despesaModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErroModel(HttpContext));
        }

        var despesa = _mapper.Map<Despesa>(despesaModel);

        await _despesaServico.AtualizarDespesa(despesa);

        return Ok(_mapper.Map<DespesaModel>(despesa));
    }

    [HttpGet("/api/ObterDespesa")]
    [Produces("application/json")]
    public async Task<IActionResult> ObterDespesa(int id)
    {
        var despesa = await _interfaceDespesa.GetEntityById(id);

        if (despesa == null)
            return NotFound();

        return Ok(_mapper.Map<DespesaModel>(despesa));
    }

    [HttpDelete("/api/DeletarDespesa")]
    [Produces("application/json")]
    public async Task<IActionResult> DeletarDespesa(int id)
    {
        try
        {
            var despesa = await _interfaceDespesa.GetEntityById(id);

            if (despesa == null)
                return NotFound();

            await _interfaceDespesa.Delete(despesa);
        }
        catch (Exception)
        {
            return Ok(false);
        }
        return Ok(true);
    }

    [HttpGet("/api/CarregaGraficos")]
    [Produces("application/json")]
    public async Task<IActionResult> CarregaGraficos(string emailUsuario)
        => Ok(await _despesaServico.CarregaGraficos(emailUsuario));
}