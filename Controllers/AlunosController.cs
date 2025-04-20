using AlunoApiFullStack.Models;
using AlunoApiFullStack.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlunoApiFullStack.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AlunosController : ControllerBase
{
    private readonly IAlunoService _alunoService;

    public AlunosController(IAlunoService alunoService)
    {
        _alunoService = alunoService;
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
    {
        try
        {
            var alunos = await _alunoService.GetAlunos();
            return Ok(alunos);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter alunos.");
        }
    }

    [HttpGet("alunoPorNome")]
    public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunosByName([FromQuery] string nome)
    {
        try
        {
            var alunos = await _alunoService.GetAlunosByNome(nome);

            if (!alunos.Any())
                return NotFound($"Não existe aluno com o critério {nome}");

            return Ok(alunos);
        }
        catch
        {
            return BadRequest($"Erro ao obter aluno pelo nome {nome}.");
        }
    }

    [HttpGet("{id}", Name = "GetAluno")]
    public async Task<ActionResult<Aluno>> GetAlunoById(int id)
    {
        try
        {
            var aluno = await _alunoService.GetAluno(id);

            if (aluno == null)
                return NotFound($"Não existe aluno com o ID {id}");

            return Ok(aluno);
        }
        catch
        {
            return BadRequest("Erro ao obter aluno pelo ID.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> CriarAluno(Aluno aluno)
    {
        try
        {
            await _alunoService.CreateAluno(aluno);
            return CreatedAtRoute("GetAluno", new { id = aluno.Id }, aluno);
        }
        catch
        {
            return BadRequest("Erro ao criar aluno.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> EditAluno(int id, [FromBody] Aluno aluno)
    {
        try
        {
            if (aluno.Id == id)
            {
                await _alunoService.UpdateAluno(aluno);
                return Ok("Aluno atualizado com sucesso.");
            }
            return BadRequest("O ID do aluno não corresponde.");
        }
        catch
        {
            return BadRequest("Erro ao atualizar aluno.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAluno(int id)
    {
        try
        {
            var aluno = await _alunoService.GetAluno(id);
            if (aluno != null)
            {
                await _alunoService.DeleteAluno(aluno);
                return Ok("Aluno deletado com sucesso.");
            }
            return NotFound($"Não existe aluno com o ID {id}");
        }
        catch
        {
            return BadRequest("Erro ao deletar aluno.");
        }
    }
}
