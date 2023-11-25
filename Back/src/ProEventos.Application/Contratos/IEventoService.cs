using System;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;

public interface IEventoService
{
    Task<EventoDto> AddEvento(int userId, EventoDto model);
    Task<EventoDto> UpdateEvento(int userId, int EventoId, EventoDto model);
    Task<bool> DeleteEvento(int userId, int EventoId);
    Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
    Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
    Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
    void DeleteImage(int eventoId, string imageName);
    Task<string> SaveImage(IFormFile image);
}
