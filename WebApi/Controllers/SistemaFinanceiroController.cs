using AutoMapper;
using Domain.Interfaces.InterfaceServicos;
using Domain.Interfaces.ISistemaFinanceiro;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SistemaFinanceiroController : ControllerBase
{
    private readonly InterfaceSistemaFinanceiro _interfaceSistemaFinanceiro;
    private readonly ISistemaFinanceiroServico _iSistemaFinanceiroServico;
    private readonly IMapper _mapper;

    public SistemaFinanceiroController(
        InterfaceSistemaFinanceiro interfaceSistemaFinanceiro,
        ISistemaFinanceiroServico iSistemaFinanceiroServico,
        IMapper mapper)
    {
        _interfaceSistemaFinanceiro = interfaceSistemaFinanceiro;
        _iSistemaFinanceiroServico = iSistemaFinanceiroServico;
        _mapper = mapper;
    }

    [HttpGet("/api/ListaSistemaUsuario")]
    [Produces("application/json")]
    public async Task<IActionResult> ListaSistemaUsuario(string emailUsuario)
    {
        var sistemasFinanceiros = await _interfaceSistemaFinanceiro.ListarSistemasUsuario(emailUsuario);

        return Ok(_mapper.Map<IList<SistemaFinanceiroModel>>(sistemasFinanceiros));
    }


    [HttpPost("/api/AdicionarSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<IActionResult> AdicionarSistemaFinanceiro(SistemaFinanceiroModel sistemaFinanceiroModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErroModel(HttpContext));
        }

        var sistemaFinanceiro = _mapper.Map<SistemaFinanceiro>(sistemaFinanceiroModel);

        await _iSistemaFinanceiroServico.AdicionarSistemaFinanceiro(sistemaFinanceiro);

        return Ok(_mapper.Map<SistemaFinanceiroModel>(sistemaFinanceiro));
    }

    [HttpPut("/api/AtualizarSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarSistemaFinanceiro(SistemaFinanceiroModel sistemaFinanceiroModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErroModel(HttpContext));
        }

        var sistemaFinanceiro = _mapper.Map<SistemaFinanceiro>(sistemaFinanceiroModel);

        await _iSistemaFinanceiroServico.AtualizarSistemaFinanceiro(sistemaFinanceiro);

        return Ok(_mapper.Map<SistemaFinanceiroModel>(sistemaFinanceiro));
    }

    [HttpGet("/api/ObterSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<IActionResult> ObterSistemaFinanceiro(int id)
    {
        var sistemaFinanceiro = await _interfaceSistemaFinanceiro.GetEntityById(id);

        if (sistemaFinanceiro == null)
            return BadRequest("Sistema Financeiro não encontrado");

        return Ok(_mapper.Map<SistemaFinanceiroModel>(sistemaFinanceiro));
    }

    [HttpDelete("/api/DeleteSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteSistemaFinanceiro(int id)
    {
        try
        {
            var sistemaFinanceiro = await _interfaceSistemaFinanceiro.GetEntityById(id);

            if (sistemaFinanceiro == null)
                return BadRequest("Sistema Financeiro não encontrado");

            await _interfaceSistemaFinanceiro.Delete(sistemaFinanceiro);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao deletar categoria: {ex.Message}");
        }
        return Ok(true);
    }

    [HttpPost("/api/ExecuteCopiaDespesasSistemaFinanceiro")]
    [Produces("application/json")]
    public async Task<IActionResult> ExecuteCopiaDespesasSistemaFinanceiro()
       => Ok(await _interfaceSistemaFinanceiro.ExecuteCopiaDespesasSistemaFinanceiro());
}