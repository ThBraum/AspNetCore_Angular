using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Contratos;

public interface ILotePersistence
{
    Task<LoteModel[]> GetLotesByEventoIdAsync(int eventoId);
    Task<LoteModel> GetLoteByIdsAsync(int eventoId, int loteId);
}
