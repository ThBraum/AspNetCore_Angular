namespace ProEventos.Application.Dtos;

public class EventoDto //Não expoe o que dominio possui para quem consumir
{
    public int Id { get; set; }
    public string? Local { get; set; }
    public DateTime? DataEvento { get; set; }
    public string? Tema { get; set; }
    public int? QtdPessoas { get; set; }
    public string? ImageURL { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    // public IEnumerable<LoteModel>? Lotes { get; set; }
    // public IEnumerable<RedeSocialModel>? RedesSociais { get; set; }
    // public IEnumerable<PalestranteEventoModel>? PalestrantesEventos { get; set; }
}