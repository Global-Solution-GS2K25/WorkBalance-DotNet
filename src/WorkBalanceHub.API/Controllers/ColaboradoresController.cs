using Microsoft.AspNetCore.Mvc;
using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Application.Services;
using System.Net;

namespace WorkBalanceHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ColaboradoresController : ControllerBase
{
    private readonly IColaboradorService _colaboradorService;
    private readonly ILogger<ColaboradoresController> _logger;

    public ColaboradoresController(IColaboradorService colaboradorService, ILogger<ColaboradoresController> logger)
    {
        _colaboradorService = colaboradorService;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo colaborador
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ColaboradorDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ColaboradorDto>> Criar([FromBody] CriarColaboradorDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var colaborador = await _colaboradorService.CriarAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = colaborador.Id }, colaborador);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Erro ao criar colaborador",
                Detail = ex.Message,
                Status = (int)HttpStatusCode.BadRequest
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Dados inválidos",
                Detail = ex.Message,
                Status = (int)HttpStatusCode.BadRequest
            });
        }
    }

    /// <summary>
    /// Obtém um colaborador por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ColaboradorDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ColaboradorDto>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var colaborador = await _colaboradorService.ObterPorIdAsync(id, cancellationToken);
        if (colaborador == null)
            return NotFound();

        return Ok(colaborador);
    }

    /// <summary>
    /// Busca colaboradores com paginação, ordenação e filtros
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResultDto<ColaboradorDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedResultDto<ColaboradorDto>>> Buscar(
        [FromQuery] SearchRequestDto request, 
        CancellationToken cancellationToken)
    {
        var resultado = await _colaboradorService.BuscarAsync(request, cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Atualiza um colaborador
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ColaboradorDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ColaboradorDto>> Atualizar(
        int id, 
        [FromBody] AtualizarColaboradorDto dto, 
        CancellationToken cancellationToken)
    {
        try
        {
            var colaborador = await _colaboradorService.AtualizarAsync(id, dto, cancellationToken);
            return Ok(colaborador);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Colaborador não encontrado",
                Detail = ex.Message,
                Status = (int)HttpStatusCode.NotFound
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Dados inválidos",
                Detail = ex.Message,
                Status = (int)HttpStatusCode.BadRequest
            });
        }
    }

    /// <summary>
    /// Remove (desativa) um colaborador
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Deletar(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _colaboradorService.DeletarAsync(id, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Obtém colaboradores por equipe
    /// </summary>
    [HttpGet("equipe/{equipeId}")]
    [ProducesResponseType(typeof(IEnumerable<ColaboradorDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<ColaboradorDto>>> ObterPorEquipe(
        int equipeId, 
        CancellationToken cancellationToken)
    {
        var colaboradores = await _colaboradorService.ObterPorEquipeIdAsync(equipeId, cancellationToken);
        return Ok(colaboradores);
    }
}

