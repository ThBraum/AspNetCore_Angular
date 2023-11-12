using ProEventos.Domain.Models;

public interface IEventoService
{
    Task<EventoModel> AddEvento(EventoModel model);
    Task<EventoModel> UpdateEvento(int EventoId, EventoModel model);
    Task<bool> DeleteEvento(int EventoId);
    Task<EventoModel[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
    Task<EventoModel[]> GetAllEventosAsync(bool includePalestrantes = false);
    Task<EventoModel> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);
}
