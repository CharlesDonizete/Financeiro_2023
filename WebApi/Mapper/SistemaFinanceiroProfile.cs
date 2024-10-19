using AutoMapper;
using Entities.Entidades;
using WebApi.Models;

namespace WebApi.Mapper
{
    public class SistemaFinanceiroProfile : Profile
    {
        public SistemaFinanceiroProfile() {                

            CreateMap<SistemaFinanceiroModel, SistemaFinanceiro>().ReverseMap();
        }
    }
}
