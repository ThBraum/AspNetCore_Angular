using ProEventos.Domain.Models;

public class PalestrateEventoModel
{
    public int PalestranteId { get; set; }
    public PalestranteModel? Palestrante { get; set; }
    public int EventoId { get; set; }
    public EventoModel? Evento { get; set; }
}
