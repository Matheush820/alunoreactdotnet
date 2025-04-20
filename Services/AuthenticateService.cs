using Microsoft.AspNetCore.Identity;

namespace AlunoApiFullStack.Services;

public class AuthenticateService : IAuthenticate
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthenticateService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<bool> Authenticate(string email, string password)
    {
        // Tentando autenticar com o nome de usuário (que será o email)
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
        return result.Succeeded;
    }

    public async Task Logout()
    {
        // Fazendo logout do usuário
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> RegisterUser(string email, string password)
    {
        try
        {
            // Verificar se o e-mail já existe
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return false;  // Retorna falso se o e-mail já estiver registrado
            }

            var appUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(appUser, password);

            if (!result.Succeeded)
            {
                // Logar todos os erros para depuração
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Erro: {error.Description}");  // Pode ser substituído por log mais adequado
                }
                return false;  // Retorna falso se a criação do usuário falhar
            }

            // Caso a criação do usuário seja bem-sucedida, realizar login
            await _signInManager.SignInAsync(appUser, false);
            return result.Succeeded;  // Retorna verdadeiro se o usuário foi criado com sucesso
        }
        catch (Exception ex)
        {
            // Captura exceções e exibe no console
            Console.WriteLine($"Erro ao registrar o usuário: {ex.Message}");
            return false;
        }
    }


}
