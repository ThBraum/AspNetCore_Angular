using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Contratos;

public interface IPalestrantePersistence
{
    Task<PalestranteModel[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false);
    Task<PalestranteModel[]> GetAllPalestrantesAsync(bool includeEventos = false);
    Task<PalestranteModel> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false);
}
