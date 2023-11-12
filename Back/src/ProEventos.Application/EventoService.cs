using ProEventos.Domain.Models;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application;

public class EventoService : IEventoService
{
    private readonly IGeralPersistence _geralPersistence;
    private readonly IEventoPersistence _eventoPersistence;
    public EventoService(IGeralPersistence geralPersistence, IEventoPersistence eventoPersistence)
    {
        _eventoPersistence = eventoPersistence;
        _geralPersistence = geralPersistence;

    }

    public async Task<EventoModel> AddEvento(EventoModel model)
    {
        try
        {
            _geralPersistence.Add<EventoModel>(model);
            if (await _geralPersistence.SaveChangesAsync())
            {
                return await _eventoPersistence.GetEventoByIdAsync(model.Id, false);
            }
            return null;
        }
        catch (System.Exception e)
        {
            
            throw new Exception(e.Message);
        }
    }

    public async Task<EventoModel> UpdateEvento(int EventoId, EventoModel model)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(EventoId, false);
            if (evento == null) throw new Exception("Evento não encontrado");
            model.Id = EventoId;

            _geralPersistence.Update<EventoModel>(model);
            if (await _geralPersistence.SaveChangesAsync())
            {
                return await _eventoPersistence.GetEventoByIdAsync(evento.Id, false);
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
            var evento = await _eventoPersistence.GetEventoByIdAsync(EventoId);
        if (evento == null) throw new Exception("Evento não encontrado");
        
        _geralPersistence.Delete<EventoModel>(evento);
        return await _geralPersistence.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
            
            throw new Exception(e.Message);
        }
    }

    public async Task<EventoModel[]> GetAllEventosAsync(bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersistence.GetAllEventosAsync(includePalestrantes);
            if (eventos == null) return null;

            return eventos;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<EventoModel[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersistence.GetAllEventosByTemaAsync(tema, includePalestrantes);
            if (eventos == null) return null;

            return eventos;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<EventoModel> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(eventoId, includePalestrantes);
            if (evento == null) return null;

            return evento;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
