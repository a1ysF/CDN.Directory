using AutoMapper;
using CDN.Directory.Core.DTOs;
using CDN.Directory.Core.Entities;

namespace CDN.Directory.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Member, MemberDto>()
                .ForMember(dest => dest.Skillsets, opt => opt.MapFrom(src =>
                    src.MemberSkillsets != null
                        ? src.MemberSkillsets.Select(ms => ms.Skillset.Name).ToList()
                        : new List<string>()))
                .ForMember(dest => dest.Hobbies, opt => opt.MapFrom(src =>
                    src.MemberHobbies != null
                        ? src.MemberHobbies.Select(mh => mh.Hobby.Name).ToList()
                        : new List<string>()));

            CreateMap<CreateMemberDto, Member>();
            CreateMap<UpdateMemberDto, Member>();
        }
    }
}
