using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application;

public class LoteService : ILoteService
{
    private readonly IGeralPersistence _geralPersistence;
    private readonly ILotePersistence _lotePersistence;
    private readonly IMapper _mapper;
    public LoteService(IGeralPersistence geralPersistence, ILotePersistence lotePersistence, IMapper mapper)
    {
        _lotePersistence = lotePersistence;
        _geralPersistence = geralPersistence;
        _mapper = mapper;
    }

    public async Task<bool> DeleteLote(int eventoId, int loteId)
    {
        try
        {
            var lote = await _lotePersistence.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) throw new Exception("Lote não encontrado");

            _geralPersistence.Delete<LoteModel>(lote);
            return await _geralPersistence.SaveChangesAsync();
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
    {
        try
        {
            var lotes = await _lotePersistence.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null) return null;

            var resultado = _mapper.Map<LoteDto[]>(lotes);
            return resultado;
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<LoteDto> GetLotesByIdsAsync(int eventoId, int loteId)
    {
        try
        {
            var lote = await _lotePersistence.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) return null;

            var resultado = _mapper.Map<LoteDto>(lote);
            return resultado;
        }
        catch (System.Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task AddLotes(int eventoId, LoteDto model)
    {
        try
        {
            var lote = _mapper.Map<LoteModel>(model);
            lote.EventoId = eventoId;
            _geralPersistence.Add<LoteModel>(lote);

            await _geralPersistence.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
    {
        try
        {
            var lotes = await _lotePersistence.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null) return null;

            foreach (var model in models)
            {
                if (model.Id == 0) //Se o Id for igual a 0, então é um novo lote
                {
                    await AddLotes(eventoId, model);
                }
                else //Se o Id for diferente de 0, então é um lote existente
                {
                    var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                    model.EventoId = eventoId;
                    _mapper.Map(model, lote); //Mapeia o model para o lote
                    _geralPersistence.Update<LoteModel>(lote);
                    await _geralPersistence.SaveChangesAsync();
                }
            }
            var loteRetorno = await _lotePersistence.GetLotesByEventoIdAsync(eventoId);
            return _mapper.Map<LoteDto[]>(loteRetorno);
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}