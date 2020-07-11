using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School_API_24
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Organization, OrganizationDto>()
                .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(x => string.Join(", ", x.Address, x.Country)));

            CreateMap<User, UserDto>();

            CreateMap<OrganizationForCreationDto, Organization>();

            CreateMap<UserForCreationDto, User>();

            CreateMap<UserForUpdateDto, User>();

            CreateMap<UserForUpdateDto, User>().ReverseMap();
        }
    }
}
