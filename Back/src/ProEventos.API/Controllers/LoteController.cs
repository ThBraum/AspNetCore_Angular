using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LoteController : Controller
{
    private readonly ILoteService _loteService;
    public LoteController(ILoteService loteService)
    {
        _loteService = loteService;
    }


    [HttpGet("{loteId}", Name = "GetLotesById")]
    public async Task<ActionResult<EventoDto[]>> Get(int loteId)
    {
        try
        {
            var lotes = await _loteService.GetLotesByEventoIdAsync(loteId);
            if (lotes == null) return NoContent();

            return Ok(lotes);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpPut("{eventoId}")]
    public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
    {
        try
        {
            var lotes = await _loteService.SaveLotes(eventoId, models);
            if (lotes == null) return NoContent();

            return Ok(lotes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
        }
    }

    [HttpDelete("{eventoId}/{loteId}", Name = "DeleteLote")]
    public async Task<ActionResult<EventoDto>> Delete(int eventoId, int loteId)
    {
        try
        {
            var lotes = await _loteService.GetLotesByIdsAsync(eventoId, loteId);
            if (lotes == null) return NoContent();

            if (await _loteService.DeleteLote(lotes.EventoId, lotes.Id)) return Ok(lotes);

            return BadRequest("Lote não deletado");
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}