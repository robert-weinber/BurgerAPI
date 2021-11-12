using AutoMapper;
using BurgerAPI.Models;
using BurgerAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurgerAPI.BurgerMapper
{
    public class BurgerMappings : Profile
    {
        public BurgerMappings()
        {
            CreateMap<Place, PlaceDto>().ReverseMap();
            CreateMap<Place, PlaceCreateDto>().ReverseMap();
            CreateMap<Place, PlaceUpdateDto>().ReverseMap();
            CreateMap<Burger, BurgerDto>().ReverseMap();
            CreateMap<Burger, BurgerCreateDto>().ReverseMap();
            CreateMap<Burger, BurgerUpdateDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
        }
    }
}
