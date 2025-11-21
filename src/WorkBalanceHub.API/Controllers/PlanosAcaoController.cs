using Microsoft.AspNetCore.Mvc;
using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Application.Services;
using System.Net;

namespace WorkBalanceHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PlanosAcaoController : ControllerBase
{
    private readonly IPlanoAcaoService _planoAcaoService;
    private readonly ILogger<PlanosAcaoController> _logger;

    public PlanosAcaoController(IPlanoAcaoService planoAcaoService, ILogger<PlanosAcaoController> logger)
    {
        _planoAcaoService = planoAcaoService;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo plano de ação
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PlanoAcaoDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PlanoAcaoDto>> Criar([FromBody] CriarPlanoAcaoDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var plano = await _planoAcaoService.CriarAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = plano.Id }, plano);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Erro ao criar plano de ação",
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
    /// Obtém um plano de ação por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlanoAcaoDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<PlanoAcaoDto>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var plano = await _planoAcaoService.ObterPorIdAsync(id, cancellationToken);
        if (plano == null)
            return NotFound();

        return Ok(plano);
    }

    /// <summary>
    /// Busca planos de ação com paginação, ordenação e filtros
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResultDto<PlanoAcaoDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedResultDto<PlanoAcaoDto>>> Buscar(
        [FromQuery] SearchRequestDto request, 
        CancellationToken cancellationToken)
    {
        var resultado = await _planoAcaoService.BuscarAsync(request, cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Atualiza um plano de ação
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PlanoAcaoDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<PlanoAcaoDto>> Atualizar(
        int id, 
        [FromBody] AtualizarPlanoAcaoDto dto, 
        CancellationToken cancellationToken)
    {
        try
        {
            var plano = await _planoAcaoService.AtualizarAsync(id, dto, cancellationToken);
            return Ok(plano);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Plano de ação não encontrado",
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
    /// Remove um plano de ação
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Deletar(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _planoAcaoService.DeletarAsync(id, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Altera o status de um plano de ação
    /// </summary>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(PlanoAcaoDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<PlanoAcaoDto>> AlterarStatus(
        int id, 
        [FromBody] AlterarStatusPlanoAcaoDto dto, 
        CancellationToken cancellationToken)
    {
        try
        {
            var plano = await _planoAcaoService.AlterarStatusAsync(id, dto, cancellationToken);
            return Ok(plano);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Plano de ação não encontrado",
                Detail = ex.Message,
                Status = (int)HttpStatusCode.NotFound
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Erro ao alterar status",
                Detail = ex.Message,
                Status = (int)HttpStatusCode.BadRequest
            });
        }
    }

    /// <summary>
    /// Obtém planos de ação por equipe
    /// </summary>
    [HttpGet("equipe/{equipeId}")]
    [ProducesResponseType(typeof(IEnumerable<PlanoAcaoDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<PlanoAcaoDto>>> ObterPorEquipe(
        int equipeId, 
        CancellationToken cancellationToken)
    {
        var planos = await _planoAcaoService.ObterPorEquipeIdAsync(equipeId, cancellationToken);
        return Ok(planos);
    }
}

