using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Helpers;

public class ProEventosProfile : Profile //Classe de mapeamento
{
    public ProEventosProfile()
    {
        CreateMap<EventoModel, EventoDto>().ReverseMap();
        CreateMap<LoteModel, LoteDto>().ReverseMap();
        CreateMap<RedeSocialModel, RedeSocialDto>().ReverseMap();
        CreateMap<PalestranteModel, PalestranteDto>().ReverseMap();
    }
}