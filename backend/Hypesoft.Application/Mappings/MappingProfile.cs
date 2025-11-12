using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;
using Hypesoft.Application.Commands;

namespace Hypesoft.Application.Mappings
{
    /// <summary>
    /// Define as regras de mapeamento para o AutoMapper.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductCommand, Product>();
        }
    }
}