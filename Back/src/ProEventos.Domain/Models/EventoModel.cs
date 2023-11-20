using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProEventos.Domain.Identity;

namespace ProEventos.Domain.Models;

[Table("Eventos")] //Especifica que a tabela terá o nome Eventos
public class EventoModel
{
    //[Key]
    public int Id { get; set; }
    public string Local { get; set; }
    public DateTime DataEvento { get; set; }
    [Required, MaxLength(50)]
    public string Tema { get; set; }
    public int QtdPessoas { get; set; }
    public string ImagemURL { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public IEnumerable<LoteModel> Lotes { get; set; }
    public IEnumerable<RedeSocialModel> RedesSociais { get; set; }
    public IEnumerable<PalestranteEventoModel> PalestrantesEventos { get; set; }
}
