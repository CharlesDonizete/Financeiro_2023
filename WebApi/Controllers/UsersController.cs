using Entities.Entidades;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPut("/api/AtualizaUsuario/{id}")]
        public async Task<IActionResult> AtualizaUsuario(string id, [FromBody] LoginModel login)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.cpf))
                return BadRequest("Faltam alguns dados.");
            
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");
            
            user.CPF = login.cpf;

            //// Atualiza a senha se fornecida
            //if (!string.IsNullOrWhiteSpace(login.senha))
            //{
            //    var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, login.senha);
            //    user.PasswordHash = newPasswordHash;
            //}

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Ok("Usuário atualizado com sucesso!");
            else
                return BadRequest(result.Errors);            
        }


        [HttpDelete("/api/DeletaUsuario/{id}")]
        public async Task<IActionResult> DeletaUsuario(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID do usuário não fornecido.");            

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");
            
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return Ok("Usuário deletado com sucesso!");
            else
                return BadRequest(result.Errors);            
        }

        [HttpGet("/api/ListaUsuarios")]
        public IActionResult ListaUsuarios()
        {
            var users = _userManager.Users.ToList();
            
            var simplifiedUserList = users.Select(user => new
            {
                UserId = user.Id,
                Email = user.Email,
                CPF = user.CPF                
            });

            return Ok(simplifiedUserList);
        }
    }
}
