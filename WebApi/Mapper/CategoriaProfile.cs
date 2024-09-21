using AutoMapper;
using Entities.Entidades;
using WebApi.Models;

namespace WebApi.Mapper
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile() {
            CreateMap<Categoria, CategoriaModel>().ReverseMap();
        }
    }
}
