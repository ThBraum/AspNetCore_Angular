using ProEventos.Domain.Models;

public class LoteModel
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public decimal? Preco { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int? Quantidade { get; set; }
    public int? EventoId { get; set; }
    public EventoModel? Evento { get; set; }
}