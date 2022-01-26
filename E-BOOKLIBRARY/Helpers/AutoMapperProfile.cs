using AutoMapper;
using E_BOOKLIBRARY.DTOs;
using E_BOOKLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, UserToReturnDto>();
        }
    }
}
