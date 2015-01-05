using AutoMapper;
using Core.Domain.Model;
using Dto.ApiResponses.ContactResponses;

namespace WebApp.Initialization.Automapper
{
    public class EntityToResponseProfile : Profile
    {
        protected override void Configure()
        {
            //.IgnoreAllUnmapped();

            // contact
            CreateMap<Contact, ContactResponse>()
                .ForMember(x => x.ContactTypeValue, o => o.MapFrom(x => x.ContactType.Value))
                .ForMember(x => x.PrimaryLanguageValue, o => o.MapFrom(x => x.PrimaryLanguage.Value))
                .ForMember(x => x.SecondaryLanguageValue, o => o.MapFrom(x => x.SecondaryLanguage.Value))
                .ForMember(x => x.CreatedByName, o => o.Ignore())
                .ForMember(x => x.ModifiedByName, o => o.Ignore());
        }
    }
}