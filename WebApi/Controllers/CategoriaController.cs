﻿using Domain.Interfaces.ICategoria;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
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
        public async Task<object> ListarCategoriasUsuario(string emailUsuario)
        {
            return await _interfaceCategoria.ListarCategoriasUsuario(emailUsuario);
        }

        [HttpPost("/api/AdicionarCategoria")]
        [Produces("application/json")]
        public async Task<object> AdicionarCategoria(Categoria categoria)
        {
            await _categoriaServico.AdicionarCategoria(categoria);

            return categoria;
        }

        [HttpPut("/api/AtualizarCategoria")]
        [Produces("application/json")]
        public async Task<object> AtualizarCategoria(Categoria categoria)
        {             
            await _categoriaServico.AtualizarCategoria(categoria);

            return categoria;
        }

        [HttpGet("/api/ObterCategoria")]
        [Produces("application/json")]
        public async Task<object> ObterCategoria(int id)
        {            
            return await _interfaceCategoria.GetEntityById(id);
        }

        [HttpDelete("/api/DeletarCategoria")]
        [Produces("application/json")]
        public async Task<object> DeletarCategoria(int id)
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
}
