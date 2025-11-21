using Microsoft.AspNetCore.Mvc;
using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Application.Services;
using System.Net;

namespace WorkBalanceHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EquipesController : ControllerBase
{
    private readonly IEquipeService _equipeService;
    private readonly ILogger<EquipesController> _logger;

    public EquipesController(IEquipeService equipeService, ILogger<EquipesController> logger)
    {
        _equipeService = equipeService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova equipe
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EquipeDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<EquipeDto>> Criar([FromBody] CriarEquipeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var equipe = await _equipeService.CriarAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = equipe.Id }, equipe);
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
    /// Obtém uma equipe por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EquipeDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<EquipeDto>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var equipe = await _equipeService.ObterPorIdAsync(id, cancellationToken);
        if (equipe == null)
            return NotFound();

        return Ok(equipe);
    }

    /// <summary>
    /// Busca equipes com paginação, ordenação e filtros
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResultDto<EquipeDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedResultDto<EquipeDto>>> Buscar(
        [FromQuery] SearchRequestDto request, 
        CancellationToken cancellationToken)
    {
        var resultado = await _equipeService.BuscarAsync(request, cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Atualiza uma equipe
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EquipeDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<EquipeDto>> Atualizar(
        int id, 
        [FromBody] AtualizarEquipeDto dto, 
        CancellationToken cancellationToken)
    {
        try
        {
            var equipe = await _equipeService.AtualizarAsync(id, dto, cancellationToken);
            return Ok(equipe);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Equipe não encontrada",
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
    /// Remove (desativa) uma equipe
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Deletar(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _equipeService.DeletarAsync(id, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}

