using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos;

public class EventoDto //Não expoe o que dominio possui para quem consumir
{
    public int Id { get; set; }
    public string Local { get; set; }
    public DateTime DataEvento { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [MaxLength(50, ErrorMessage = "O campo {0} deve ter no máximo 50 caracteres")]
    public string Tema { get; set; }
    [Range(1, 120000, 
    ErrorMessage = "O campo {0} deve ter no mínimo 1 e no máximo 120.000")]
    [DefaultValue(1)]
    public int QtdPessoas { get; set; }
    [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$",
    ErrorMessage = "Não é uma imagem válida. (gif, jpg, jpeg, bmp ou png)")]
    [DefaultValue("foto.png")]
    public string ImagemURL { get; set; }
    [Phone(ErrorMessage = "O campo {0} está em formato inválido"),
    DefaultValue("(00) 00000-0000")]
    public string Telefone { get; set; }
    [Display(Name = "e-mail"),
    EmailAddress(ErrorMessage = "O campo {0} está em formato inválido"),
    Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Email { get; set; }
    public int UserId { get; set; }
    public UserDto UserDto { get; set; }
    public IEnumerable<LoteDto> Lotes { get; set; }
    public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
    public IEnumerable<PalestranteDto> PalestrantesEventos { get; set; }
}