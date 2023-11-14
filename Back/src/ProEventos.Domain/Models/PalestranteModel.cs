using System.ComponentModel.DataAnnotations.Schema;

namespace ProEventos.Domain.Models;

public class PalestranteModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string MiniCurriculo { get; set; }
    public string ImagemURL { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int? EventoId { get; set; }
    public IEnumerable<RedeSocialModel> RedesSociais { get; set; }
    public IEnumerable<PalestranteEventoModel> PalestrantesEventos { get; set; }
}