using AutoMapper;
using Entities.Entidades;
using WebApi.Models;

namespace WebApi.Mapper
{
    public class DespesaProfile : Profile
    {
        public DespesaProfile() {                

            CreateMap<DespesaModel, Despesa>().ReverseMap();
        }
    }
}
