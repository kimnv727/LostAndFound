using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Media;

namespace LostAndFound.Infrastructure.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Media Mapping
            CreateMap<Media, MediaReadDTO>();
            CreateMap<Media, MediaDetailReadDTO>();
            CreateMap<MediaUpdateWriteDTO, Media>();
        }
    }
}
