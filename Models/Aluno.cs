using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlunoApiFullStack.Models;

[Table("Alunos")]
public class Aluno
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MaxLength(80, ErrorMessage = "O campo Nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo Email é inválido.")]
    [MaxLength(80, ErrorMessage = "O campo Nome deve ter no máximo 100 caracteres.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "O campo Idade é obrigatório.")]
    public int Idade { get; set; }
}
