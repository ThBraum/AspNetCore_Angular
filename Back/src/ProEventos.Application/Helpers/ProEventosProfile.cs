using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Helpers;

public class ProEventosProfile : Profile //Classe de mapeamento
{
    public ProEventosProfile()
    {
        CreateMap<EventoModel, EventoDto>().ReverseMap();
    }
}