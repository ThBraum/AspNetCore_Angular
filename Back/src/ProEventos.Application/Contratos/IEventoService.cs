using ProEventos.Application.Dtos;

public interface IEventoService
{
    Task<EventoDto> AddEvento(EventoDto model);
    Task<EventoDto> UpdateEvento(int EventoId, EventoDto model);
    Task<bool> DeleteEvento(int EventoId);
    Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
    Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false);
    Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);
}
