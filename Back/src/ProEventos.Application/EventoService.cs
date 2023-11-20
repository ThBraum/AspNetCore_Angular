using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application;

public class EventoService : IEventoService
{
    private readonly IGeralPersistence _geralPersistence;
    private readonly IEventoPersistence _eventoPersistence;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _hostEnvironment;
    public EventoService(IGeralPersistence geralPersistence, IEventoPersistence eventoPersistence, IMapper mapper, IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _eventoPersistence = eventoPersistence;
        _geralPersistence = geralPersistence;
        _mapper = mapper;

    }

    public async Task<EventoDto> AddEvento(EventoDto model)
    {
        try
        {
            var evento = _mapper.Map<EventoModel>(model);

            _geralPersistence.Add<EventoModel>(evento);
            if (await _geralPersistence.SaveChangesAsync())
            {
                var eventoRetorno = await _eventoPersistence.GetEventoByIdAsync(evento.Id, false);
                return _mapper.Map<EventoDto>(eventoRetorno);
            }
            return null;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<EventoDto> UpdateEvento(int EventoId, EventoDto model)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(EventoId, false);
            if (evento == null) throw new Exception("Evento não encontrado");
            model.Id = EventoId;

            _mapper.Map(model, evento); //Mapeia o model para o evento

            _geralPersistence.Update<EventoModel>(evento); //Atualiza o evento
            if (await _geralPersistence.SaveChangesAsync())
            {
                var eventoRetorno = await _eventoPersistence.GetEventoByIdAsync(evento.Id, false);
                return _mapper.Map<EventoDto>(eventoRetorno);
            }
            return null;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> DeleteEvento(int EventoId)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(EventoId, false);
            if (evento == null) throw new Exception("Evento não encontrado");

            _geralPersistence.Delete<EventoModel>(evento);
            return await _geralPersistence.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
            throw new Exception($"Erro {e.Message}");
        }
    }

    public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersistence.GetAllEventosAsync(includePalestrantes);
            if (eventos == null) return null;

            var resultados = _mapper.Map<EventoDto[]>(eventos);

            return resultados;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersistence.GetAllEventosByTemaAsync(tema, includePalestrantes);
            if (eventos == null) return null;

            var resultados = _mapper.Map<EventoDto[]>(eventos);

            return resultados;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(eventoId, includePalestrantes);
            if (evento == null) return null;

            var resultado = _mapper.Map<EventoDto>(evento);

            return resultado;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void DeleteImage(string imageName)
    {
        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);
        if (System.IO.File.Exists(imagePath))
            System.IO.File.Delete(imagePath);
    }

    public async Task<string> SaveImage(IFormFile image)
    {
        string imageName = new String(Path.GetFileNameWithoutExtension(image.FileName)
                                        .Take(10).ToArray()).Replace(' ', '-');
        imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(image.FileName)}";

        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);
        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        return imageName;
    }
}
