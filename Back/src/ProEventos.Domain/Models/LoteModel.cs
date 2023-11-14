using System.ComponentModel.DataAnnotations.Schema;
using ProEventos.Domain.Models;

public class LoteModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int Quantidade { get; set; }
    //[ForeignKey("Eventos")] //Especifica que o campo EventoId é uma chave estrangeira para a tabela Eventos
    public int EventoId { get; set; }
    public EventoModel Evento { get; set; }
}