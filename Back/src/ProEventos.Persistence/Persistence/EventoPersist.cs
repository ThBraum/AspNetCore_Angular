using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence;

public class EventoPersist : IEventoPersistence
{
    private readonly ProEventosContext _context;

    public EventoPersist(ProEventosContext context)
    {
        _context = context;
    }

    public async Task<EventoModel[]> GetAllEventosAsync(bool includePalestrantes = false)
    {
        IQueryable<EventoModel> query = _context.Eventos.AsNoTracking().Include(e => e.Lotes).Include(e => e.RedesSociais);
        query.OrderBy(e => e.Id);

        if (includePalestrantes)
        {
            query = query.Include(e => e.PalestrantesEventos!).ThenInclude(pe => pe.Palestrante);
        }

        return await query.ToArrayAsync();
    }

    public async Task<EventoModel[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
        IQueryable<EventoModel> query = _context.Eventos.AsNoTracking().Include(e => e.Lotes).Include(e => e.RedesSociais);

        if (includePalestrantes)
        {
            query = query.Include(e => e.PalestrantesEventos!).ThenInclude(pe => pe.Palestrante);
        }

        query = query.Where(e => e.Tema.ToLower().Contains(tema.ToLower()));
        query = query.OrderBy(e => e.Id);

        return await query.ToArrayAsync();
    }

    public async Task<EventoModel?> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
        IQueryable<EventoModel> query = _context.Eventos.AsNoTracking().Include(e => e.Lotes).Include(e => e.RedesSociais);

        if (includePalestrantes)
        {
            query = query.Include(e => e.PalestrantesEventos!).ThenInclude(pe => pe.Palestrante);
        }
        query = query.Where(e => e.Id == eventoId).OrderBy(e => e.Id);
        return await query.FirstOrDefaultAsync();
    }
}