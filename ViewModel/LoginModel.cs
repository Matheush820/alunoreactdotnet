using System.ComponentModel.DataAnnotations;

namespace AlunoApiFullStack.ViewModel;

public class LoginModel
{
    [Required(ErrorMessage = "O campo é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo está em formato inválido")]
    public string Email { get; set; }
    [Required(ErrorMessage = "O campo  é obrigatório")]
    [StringLength(20, ErrorMessage = "O campo deve ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Password { get; set; }
}
