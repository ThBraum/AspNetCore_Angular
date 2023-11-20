using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence;

public class LotePersist : ILotePersistence
{
    private readonly ProEventosContext _context;

    public LotePersist(ProEventosContext context)
    {
        _context = context;
    }

    public async Task<LoteModel> GetLoteByIdsAsync(int eventoId, int loteId)
    {
        IQueryable<LoteModel> query = _context.Lotes.AsNoTracking().Where(l => l.EventoId == eventoId && l.Id == loteId);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<LoteModel[]> GetLotesByEventoIdAsync(int eventoId)
    {
        IQueryable<LoteModel> query = _context.Lotes.AsNoTracking().Where(l => l.EventoId == eventoId);

        return await query.ToArrayAsync();
    }
}