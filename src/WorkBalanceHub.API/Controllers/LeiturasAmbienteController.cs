using Microsoft.AspNetCore.Mvc;
using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Application.Services;
using System.Net;

namespace WorkBalanceHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LeiturasAmbienteController : ControllerBase
{
    private readonly ILeituraAmbienteService _leituraAmbienteService;
    private readonly ILogger<LeiturasAmbienteController> _logger;

    public LeiturasAmbienteController(ILeituraAmbienteService leituraAmbienteService, ILogger<LeiturasAmbienteController> logger)
    {
        _leituraAmbienteService = leituraAmbienteService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova leitura de ambiente
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(LeituraAmbienteDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<LeituraAmbienteDto>> Criar([FromBody] CriarLeituraAmbienteDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var leitura = await _leituraAmbienteService.CriarAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = leitura.Id }, leitura);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Erro ao criar leitura de ambiente",
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
    /// Obtém uma leitura de ambiente por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LeituraAmbienteDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<LeituraAmbienteDto>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var leitura = await _leituraAmbienteService.ObterPorIdAsync(id, cancellationToken);
        if (leitura == null)
            return NotFound();

        return Ok(leitura);
    }

    /// <summary>
    /// Busca leituras de ambiente com paginação, ordenação e filtros
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResultDto<LeituraAmbienteDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedResultDto<LeituraAmbienteDto>>> Buscar(
        [FromQuery] SearchRequestDto request, 
        CancellationToken cancellationToken)
    {
        var resultado = await _leituraAmbienteService.BuscarAsync(request, cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Remove uma leitura de ambiente
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Deletar(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _leituraAmbienteService.DeletarAsync(id, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Obtém leituras de ambiente por estação de trabalho
    /// </summary>
    [HttpGet("estacao/{estacaoTrabalhoId}")]
    [ProducesResponseType(typeof(IEnumerable<LeituraAmbienteDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<LeituraAmbienteDto>>> ObterPorEstacao(
        int estacaoTrabalhoId, 
        CancellationToken cancellationToken)
    {
        var leituras = await _leituraAmbienteService.ObterPorEstacaoTrabalhoIdAsync(estacaoTrabalhoId, cancellationToken);
        return Ok(leituras);
    }
}

