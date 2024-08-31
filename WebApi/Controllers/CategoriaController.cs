using Domain.Interfaces.ICategoria;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using FluentValidation;
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

    public CategoriaController(
        InterfaceCategoria interfaceCategoria,
        ICategoriaServico categoriaServico)
    {
        _interfaceCategoria = interfaceCategoria;
        _categoriaServico = categoriaServico;
    }

    [HttpGet("/api/ListarCategoriasUsuario")]
    [Produces("application/json")]
    public async Task<IList<Categoria>> ListarCategoriasUsuario(string emailUsuario)
        => await _interfaceCategoria.ListarCategoriasUsuario(emailUsuario);    

    [HttpPost("/api/AdicionarCategoria")]
    [Produces("application/json")]
    public async Task<IActionResult> AdicionarCategoria(CategoriaModel categoriaModel) 
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErroModel(HttpContext));
        }

        var categoria = new Categoria() { IdSistema= categoriaModel.IdSistema };

        await _categoriaServico.AdicionarCategoria(categoria);

        categoriaModel.Id = categoria.Id;

        return Ok(categoriaModel);
    }

    [HttpPut("/api/AtualizarCategoria")]
    [Produces("application/json")]
    public async Task<Categoria> AtualizarCategoria(Categoria categoria)
    {
        await _categoriaServico.AtualizarCategoria(categoria);

        return categoria;
    }

    [HttpGet("/api/ObterCategoria")]
    [Produces("application/json")]
    public async Task<Categoria> ObterCategoria(int id)
        => await _interfaceCategoria.GetEntityById(id);    

    [HttpDelete("/api/DeletarCategoria")]
    [Produces("application/json")]
    public async Task<bool> DeletarCategoria(int id)
    {
        try
        {
            var categoria = await _interfaceCategoria.GetEntityById(id);

            await _interfaceCategoria.Delete(categoria);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
}