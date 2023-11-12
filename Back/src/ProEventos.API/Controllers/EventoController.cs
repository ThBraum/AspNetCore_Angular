using Microsoft.AspNetCore.Mvc;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;

namespace ProEventos.API.Controllers;
[Route("api/[controller]")]
public class EventoController : Controller
{
    private readonly IEventoService _eventoService;
    public EventoController(IEventoService eventoService)
    {
        _eventoService = eventoService;
    }

    [HttpGet(Name = "GetEvento")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync(true);
            if (eventos == null) return NotFound("Nenhum evento encontrado");

            return Ok(eventos);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpGet("{id}", Name = "GetEventoById")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(id, true);
            if (evento == null) return NotFound("Nenhum evento encontrado");

            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpGet("{tema}/tema", Name = "GetEventoByTema")]
    public async Task<IActionResult> Get(string tema)
    {
        try
        {
            var evento = await _eventoService.GetAllEventosByTemaAsync(tema, true);
            if (evento == null) return NotFound("Nenhum evento encontrado");

            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpPost(Name = "AddEvento")]
    public async Task<IActionResult> Post(EventoModel model)
    {
        try
        {
            var evento = await _eventoService.AddEvento(model);
            if (evento == null) return BadRequest("Erro ao tentar adicionar evento");

            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpPut("{id}", Name = "UpdateEvento")]
    public async Task<IActionResult> Put(int id, EventoModel model)
    {
        try
        {
            var evento = await _eventoService.UpdateEvento(id, model);
            if (evento == null) return BadRequest("Erro ao tentar atualizar evento");

            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpDelete("{id}", Name = "DeleteEvento")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (await _eventoService.DeleteEvento(id))
            {
                return Ok("Deletado");
            }
            else
            {
                return BadRequest("Evento não deletado");
            }
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }


}