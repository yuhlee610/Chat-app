using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Configuration
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<User, UserInputDTO>().ReverseMap();
            CreateMap<Group, GroupInputDTO>().ReverseMap();
        }
    }
}
