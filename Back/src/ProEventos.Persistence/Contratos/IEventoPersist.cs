using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Contratos;

public interface IEventoPersistence
{
    Task<EventoModel[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
    Task<EventoModel[]> GetAllEventosAsync(bool includePalestrantes = false);
    Task<EventoModel> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);
}
