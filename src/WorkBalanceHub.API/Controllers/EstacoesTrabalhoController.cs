using Microsoft.AspNetCore.Mvc;
using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Application.Services;
using System.Net;

namespace WorkBalanceHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EstacoesTrabalhoController : ControllerBase
{
    private readonly IEstacaoTrabalhoService _estacaoTrabalhoService;
    private readonly ILogger<EstacoesTrabalhoController> _logger;

    public EstacoesTrabalhoController(IEstacaoTrabalhoService estacaoTrabalhoService, ILogger<EstacoesTrabalhoController> logger)
    {
        _estacaoTrabalhoService = estacaoTrabalhoService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova estação de trabalho
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EstacaoTrabalhoDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<EstacaoTrabalhoDto>> Criar([FromBody] CriarEstacaoTrabalhoDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var estacao = await _estacaoTrabalhoService.CriarAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = estacao.Id }, estacao);
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
    /// Obtém uma estação de trabalho por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EstacaoTrabalhoDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<EstacaoTrabalhoDto>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var estacao = await _estacaoTrabalhoService.ObterPorIdAsync(id, cancellationToken);
        if (estacao == null)
            return NotFound();

        return Ok(estacao);
    }

    /// <summary>
    /// Busca estações de trabalho com paginação, ordenação e filtros
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResultDto<EstacaoTrabalhoDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedResultDto<EstacaoTrabalhoDto>>> Buscar(
        [FromQuery] SearchRequestDto request, 
        CancellationToken cancellationToken)
    {
        var resultado = await _estacaoTrabalhoService.BuscarAsync(request, cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Atualiza uma estação de trabalho
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EstacaoTrabalhoDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<EstacaoTrabalhoDto>> Atualizar(
        int id, 
        [FromBody] AtualizarEstacaoTrabalhoDto dto, 
        CancellationToken cancellationToken)
    {
        try
        {
            var estacao = await _estacaoTrabalhoService.AtualizarAsync(id, dto, cancellationToken);
            return Ok(estacao);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Estação de trabalho não encontrada",
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
    /// Remove (desativa) uma estação de trabalho
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Deletar(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _estacaoTrabalhoService.DeletarAsync(id, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}

