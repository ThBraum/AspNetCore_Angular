using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EventoController : Controller
{
    private readonly IEventoService _eventoService;
    private readonly IWebHostEnvironment _hostEnvironment;
    public EventoController(IEventoService eventoService, IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _eventoService = eventoService;
    }

    [HttpGet(Name = "GetEvento")]
    public async Task<ActionResult<EventoDto[]>> Get()
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync(true);
            if (eventos == null) return NoContent();

            return Ok(eventos);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpGet("{id}", Name = "GetEventoById")]
    public async Task<ActionResult<EventoDto>> GetById(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(id, true);
            if (evento == null) return NoContent();

            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpGet("{tema}/tema", Name = "GetEventoByTema")]
    public async Task<ActionResult<EventoDto>> Get(string tema)
    {
        try
        {
            var evento = await _eventoService.GetAllEventosByTemaAsync(tema, true);
            if (evento == null) return NoContent();

            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpPost(Name = "AddEvento")]
    public async Task<ActionResult<EventoDto>> Post([FromBody] EventoDto model)
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

    [HttpPost("upload-image/{eventoId}", Name = "UploadImage")]
    public async Task<ActionResult<EventoDto>> UploadImage(int eventoId)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(eventoId, true);
            if (evento == null) return NoContent();
            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                _eventoService.DeleteImage(evento.ImagemURL);
                evento.ImagemURL = await _eventoService.SaveImage(file);
            }
            var eventoRetorno = await _eventoService.UpdateEvento(eventoId, evento);
            if (eventoRetorno == null) return BadRequest("Erro ao tentar atualizar evento");
            return Ok(eventoRetorno);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpPut("{id}", Name = "UpdateEvento")]
    public async Task<ActionResult<EventoDto>> Put(int id, [FromBody] EventoDto model)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(id, true);
            if (evento == null) return NoContent();

            var eventoRetorno = await _eventoService.UpdateEvento(id, model);
            if (eventoRetorno == null) return BadRequest("Erro ao tentar atualizar evento");

            return Ok(eventoRetorno);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpDelete("{id}", Name = "DeleteEvento")]
    public async Task<ActionResult<EventoDto>> Delete(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(id, true);
            if (evento == null) return NoContent();

            if (await _eventoService.DeleteEvento(id)) return Ok(evento);

            return BadRequest("Evento não deletado");
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}