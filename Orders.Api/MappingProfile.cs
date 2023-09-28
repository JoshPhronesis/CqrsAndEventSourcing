using AutoMapper;
using Orders.Api.Dtos;
using Orders.Api.Entities;

namespace Orders.Api;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderForCreateDto>().ReverseMap();
        CreateMap<Order, OrderForUpdateDto>().ReverseMap();
        CreateMap<Order, OrderForReturnDto>().ReverseMap();
    }
}