using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence;

public class PalestrantePersist : IPalestrantePersistence
{
    private readonly ProEventosContext _context;

    public PalestrantePersist(ProEventosContext context)
    {
        _context = context;
    }

    public async Task<PalestranteModel[]> GetAllPalestrantesAsync(bool includeEventos = false)
    {
        IQueryable<PalestranteModel> query = _context.Palestrantes.Include(p => p.RedesSociais);

        if (includeEventos)
        {
            query = query.Include(p => p.PalestratesEventos!).ThenInclude(pe => pe.Evento);
        }
        query.OrderBy(p => p.Id);        

        return await query.ToArrayAsync();
    }

    public async Task<PalestranteModel[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
    {
        IQueryable<PalestranteModel> query = _context.Palestrantes.Include(p => p.RedesSociais);

        if (includeEventos)
        {
            query = query.Include(p => p.PalestratesEventos!).ThenInclude(pe => pe.Evento);
        }
        query.OrderBy(p => p.Id).Where(p => p.Nome!.ToLower().Contains(nome.ToLower()));

        return await query.ToArrayAsync();
    }


    public async Task<PalestranteModel> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false)
    {
        IQueryable<PalestranteModel> query = _context.Palestrantes.Include(p => p.RedesSociais);

        if (includeEventos)
        {
            query = query.Include(e => e.PalestratesEventos!).ThenInclude(pe => pe.Evento);
        }
        query = query.OrderBy(p => p.Id).Where(p => p.Id == palestranteId);
        
        var palestrante = await query.FirstOrDefaultAsync();
        if (palestrante == null)
        {
            return null;
        }

        return palestrante;
    }
}