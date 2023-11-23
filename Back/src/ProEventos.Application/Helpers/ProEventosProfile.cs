using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Helpers;

public class ProEventosProfile : Profile //Classe de mapeamento
{
    public ProEventosProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserLoginDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
        
        CreateMap<EventoModel, EventoDto>().ReverseMap();
        CreateMap<LoteModel, LoteDto>().ReverseMap();
        CreateMap<RedeSocialModel, RedeSocialDto>().ReverseMap();
        CreateMap<PalestranteModel, PalestranteDto>().ReverseMap();
    }
}