using AutoMapper;
using Domain.Interfaces.ICategoria;
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
public class CategoriaController : ControllerBase
{
    private readonly InterfaceCategoria _interfaceCategoria;
    private readonly ICategoriaServico _categoriaServico;
    private readonly IMapper _mapper;

    public CategoriaController(
        InterfaceCategoria interfaceCategoria,
        ICategoriaServico categoriaServico,
        IMapper mapper)
    {
        _interfaceCategoria = interfaceCategoria;
        _categoriaServico = categoriaServico;
        _mapper = mapper;
    }

    [HttpGet("/api/ListarCategoriasUsuario")]
    [Produces("application/json")]
    public async Task<IActionResult> ListarCategoriasUsuario(string emailUsuario)
    {
        var categoriasUsuario = await _interfaceCategoria.ListarCategoriasUsuario(emailUsuario);

        return Ok(_mapper.Map<IList<CategoriaModel>>(categoriasUsuario));
    }


    [HttpPost("/api/AdicionarCategoria")]
    [Produces("application/json")]
    public async Task<IActionResult> AdicionarCategoria(CategoriaModel categoriaModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErroModel(HttpContext));
        }

        var categoria = _mapper.Map<Categoria>(categoriaModel);

        await _categoriaServico.AdicionarCategoria(categoria);

        return Ok(_mapper.Map<CategoriaModel>(categoria));
    }

    [HttpPut("/api/AtualizarCategoria")]
    [Produces("application/json")]
    public async Task<IActionResult> AtualizarCategoria(CategoriaModel categoriaModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErroModel(HttpContext));
        }

        var categoria = _mapper.Map<Categoria>(categoriaModel);

        await _categoriaServico.AtualizarCategoria(categoria);

        return Ok(_mapper.Map<CategoriaModel>(categoria));
    }

    [HttpGet("/api/ObterCategoria")]
    [Produces("application/json")]
    public async Task<IActionResult> ObterCategoria(int id)
    {
        var categoria = await _interfaceCategoria.GetEntityById(id);

        if (categoria == null)
            return BadRequest("Categoria não encontrada");

        return Ok(_mapper.Map<CategoriaModel>(categoria));
    }


    [HttpDelete("/api/DeletarCategoria")]
    [Produces("application/json")]
    public async Task<IActionResult> DeletarCategoria(int id)
    {
        try
        {
            var categoria = await _interfaceCategoria.GetEntityById(id);

            if (categoria == null)
                return BadRequest("Categoria não encontrada");

            await _interfaceCategoria.Delete(categoria);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao deletar categoria: {ex.Message}");
        }
        return Ok(true);
    }
}