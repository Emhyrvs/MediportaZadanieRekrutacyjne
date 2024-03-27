using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Models;
using AutoMapper;
namespace MediportaZadanieRekrutacyjne.Profiles

{
    public class TagsProfile : Profile
    {
        public TagsProfile()
        {
            // Source -> Target
            CreateMap<TagItemDto, TagItem>();
        }

    }
    
}
