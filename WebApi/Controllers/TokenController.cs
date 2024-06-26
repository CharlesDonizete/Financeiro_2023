﻿using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Token;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TokenController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] InputModel inputModel)
        {
            if (string.IsNullOrWhiteSpace(inputModel.Email) || string.IsNullOrWhiteSpace(inputModel.Password))
                return Unauthorized();

            var result = await _signInManager.PasswordSignInAsync(inputModel.Email, inputModel.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var token = new TokenJWTBuilder()
                     .AddSecurityKey(JwtSecurityKey.Create("43443FDFDF34DF34343fdf344SDFSDFSDFSDFSDF4545354345SDFGDFGDFGDFGdffgfdGDFGDGR%"))
                      .AddSubject("Controle Financeiro")
                      .AddIssuer("Teste.Securiry.Bearer")
                      .AddAudience("Teste.Securiry.Bearer")
                      .AddClaim("UsuarioAPINumero", "1")
                      .AddExpiry(20)
                      .Builder();

                return Ok(token.value);
            }

            return Unauthorized();
        }
    }
}
