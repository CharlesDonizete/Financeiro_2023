using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AdicionaUsuario")]
        public async Task<IActionResult> AdicionarUsuario([FromBody] LoginModel loginModel)
        {
            if (string.IsNullOrWhiteSpace(loginModel.email) ||
                string.IsNullOrWhiteSpace(loginModel.senha) ||
                string.IsNullOrWhiteSpace(loginModel.cpf))
                return Ok("Falta alguns dados");

            var user = new ApplicationUser
            {
                Email = loginModel.email,
                UserName = loginModel.email,
                CPF = loginModel.cpf
            };

            var result = await _userManager.CreateAsync(user, loginModel.senha);

            if (result.Errors.Any())
                return BadRequest(result.Errors);

            //Geração de confirmação
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //retorno do email
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var response_Return = await _userManager.ConfirmEmailAsync(user, code);

            if (response_Return.Succeeded)
                return Ok("Usuário Adicionado");

            return Ok("Erro ao confirmar cadastro de usuário!");
        }
    }
}
