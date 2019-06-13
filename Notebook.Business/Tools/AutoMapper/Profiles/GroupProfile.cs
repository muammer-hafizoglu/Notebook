using AutoMapper;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Tools.AutoMapper.Profiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            //CreateMap<Group, Group>()
            //    .ForMember(dest => dest.Name,
            //        opts => opts.MapFrom(
            //            src => src.Name
            //        )).ReverseMap();
        }
    }
}
