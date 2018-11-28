﻿namespace Atanet.Model.Mappings.Files
{
    using AutoMapper;
    using Atanet.Model.Data;
    using Atanet.Model.Dto;

    public class FileInfoDtoMap : Profile
    {
        public FileInfoDtoMap()
        {
            this.CreateMap<File, FileInfoDto>()
                .ForMember(x => x.ContentType, x => x.MapFrom(p => p.ContentType))
                .ForMember(x => x.Created, x => x.MapFrom(p => p.Created))
                .ForMember(x => x.FileName, x => x.MapFrom(p => p.FileName))
                .ForMember(x => x.Id, x => x.MapFrom(p => p.Id));
        }
    }
}
