using Microsoft.AspNetCore.Mvc;
using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Application.Services;
using System.Net;

namespace WorkBalanceHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CheckInsController : ControllerBase
{
    private readonly ICheckInService _checkInService;
    private readonly ILogger<CheckInsController> _logger;

    public CheckInsController(ICheckInService checkInService, ILogger<CheckInsController> logger)
    {
        _checkInService = checkInService;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo check-in
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CheckInDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CheckInDto>> Criar([FromBody] CriarCheckInDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var checkIn = await _checkInService.CriarAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(ObterPorId), new { id = checkIn.Id }, checkIn);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Erro ao criar check-in",
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
    /// Obtém um check-in por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CheckInDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<CheckInDto>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var checkIn = await _checkInService.ObterPorIdAsync(id, cancellationToken);
        if (checkIn == null)
            return NotFound();

        return Ok(checkIn);
    }

    /// <summary>
    /// Busca check-ins com paginação, ordenação e filtros
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResultDto<CheckInDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PagedResultDto<CheckInDto>>> Buscar(
        [FromQuery] SearchRequestDto request, 
        CancellationToken cancellationToken)
    {
        var resultado = await _checkInService.BuscarAsync(request, cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Atualiza um check-in
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CheckInDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<CheckInDto>> Atualizar(
        int id, 
        [FromBody] AtualizarCheckInDto dto, 
        CancellationToken cancellationToken)
    {
        try
        {
            var checkIn = await _checkInService.AtualizarAsync(id, dto, cancellationToken);
            return Ok(checkIn);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Check-in não encontrado",
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
    /// Remove um check-in
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Deletar(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _checkInService.DeletarAsync(id, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Obtém check-ins por colaborador
    /// </summary>
    [HttpGet("colaborador/{colaboradorId}")]
    [ProducesResponseType(typeof(IEnumerable<CheckInDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<CheckInDto>>> ObterPorColaborador(
        int colaboradorId, 
        CancellationToken cancellationToken)
    {
        var checkIns = await _checkInService.ObterPorColaboradorIdAsync(colaboradorId, cancellationToken);
        return Ok(checkIns);
    }

    /// <summary>
    /// Obtém indicadores de bem-estar por equipe e período
    /// </summary>
    [HttpGet("indicadores/equipe/{equipeId}")]
    [ProducesResponseType(typeof(IndicadoresBemEstarDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IndicadoresBemEstarDto>> ObterIndicadores(
        int equipeId,
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim,
        CancellationToken cancellationToken)
    {
        var indicadores = await _checkInService.ObterIndicadoresPorEquipeAsync(equipeId, dataInicio, dataFim, cancellationToken);
        return Ok(indicadores);
    }
}

