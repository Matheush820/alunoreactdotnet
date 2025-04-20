using System.ComponentModel.DataAnnotations;

namespace AlunoApiFullStack.ViewModel;

public class RegisterModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [Display(Name = "Confirmar senha")]
    [Compare("Password", ErrorMessage = "As senhas nao coincidem")]
    public string ConfirmPassword { get; set; }
}
