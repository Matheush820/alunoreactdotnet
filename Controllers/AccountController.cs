using AlunoApiFullStack.Services;
using AlunoApiFullStack.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AlunoApiFullStack.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuthenticate _authenticate;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(IConfiguration configuration, IAuthenticate authenticate, UserManager<IdentityUser> userManager)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _authenticate = authenticate ?? throw new ArgumentNullException(nameof(authenticate));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult<UserToken>> CreateUser([FromBody] RegisterModel model)
    {
        if (model.Password != model.ConfirmPassword)
        {
            ModelState.AddModelError("ConfirmPassword", "As senhas não conferem");
            return BadRequest(ModelState);  // Melhor retornar o ModelState para mostrar o erro completo.
        }

        // Verificar se o e-mail já existe
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("CreateUser", "O e-mail já está em uso.");
            return BadRequest(ModelState);  // Retorna erro se o e-mail já existir
        }

        var result = await _authenticate.RegisterUser(model.Email, model.Password);

        if (result)
        {
            return Ok($"Usuário {model.Email} criado com sucesso");
        }
        else
        {
            ModelState.AddModelError("CreateUser", "Falha no registro");
            return BadRequest(ModelState);  // Retorna o ModelState com os erros detalhados.
        }
    }

    [HttpPost("LoginUser")]
    public async Task<ActionResult<UserToken>> LoginUser([FromBody] LoginModel userInfo)
    {
        var result = await _authenticate.Authenticate(userInfo.Email, userInfo.Password);

        if (result)
        {
            return GenerateToken(userInfo);
        }
        else
        {
            return BadRequest("Login inválido");
        }
    }

    private ActionResult<UserToken> GenerateToken(LoginModel userInfo)
    {
        var claims = new[]
        {
            new Claim("email", userInfo.Email),
            new Claim("meuToken", "token do matheus"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(1);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

        return new UserToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}
