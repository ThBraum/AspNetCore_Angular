using ProEventos.Domain.Models;

public class RedeSocialModel
{
    public int Id { get; set; }
    public string? Local { get; set; }
    public string? URL { get; set; }
    public int? EventoId { get; set; }
    public EventoModel? Evento { get; set; }
    public int PalestranteId { get; set; }
    public PalestranteModel? Palestrante { get; set; }
}