using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Contratos;

public interface IEventoPersistence
{
    Task<EventoModel[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
    Task<EventoModel[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
    Task<EventoModel> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
}
