using AutoMapper;
using Common.Enumerations;
using Common.Helpers;
using Common.SystemSettings;
using Core.Commands.AppUserCommands;
using Core.Commands.AttachmentCommands;
using Core.Commands.ContactCommands;
using Core.Commands.LeadCommands;
using Core.Commands.NewsLetterCommands;
using Core.Commands.NoteCommands;
using Core.Commands.TemplateCommands;
using Dto.ApiRequests.AppUserForms;
using Dto.ApiRequests.AttachmentForms;
using Dto.ApiRequests.ConfigForms;
using Dto.ApiRequests.ContactForms;
using Dto.ApiRequests.LeadForms;
using Dto.ApiRequests.NewsLetter;
using Dto.ApiRequests.NoteForms;
using Dto.ApiRequests.TemplateForms;

namespace WebApp.Initialization.Automapper
{
    public class FormToCommandProfile : Profile
    {
        protected override void Configure()
        {
            //AppUser
            CreateMap<AddAppUserForm, AddAppUser>()
                .ForMember(x => x.Id, o => o.MapFrom(x => GuidComb.New()))
                .ForMember(x => x.UserStatus, o => o.MapFrom(x => UserStatus.FromValue(x.UserStatusValue)));

            CreateMap<UpdateAppUserForm, UpdateAppUser>()
                .ForMember(x => x.UserStatus, o => o.MapFrom(x => UserStatus.FromValue(x.UserStatusValue)));
            
            //Configuration
            CreateMap<SmsConfigForm, SmsConfig>();
            CreateMap<NetworkConfigForm, NetworkConfig>();
            CreateMap<EmailConfigForm, EmailConfig>();

            //Template
            CreateMap<AddTemplateForm, AddTemplate>()
                .ForMember(x => x.Id, o => o.MapFrom(x => GuidComb.New()));

            CreateMap<UpdateTemplateForm, UpdateTemplate>();

            //Contact
            CreateMap<AddContactForm, AddContact>()
                .ForMember(x => x.Id, o => o.MapFrom(x => GuidComb.New()))
                .ForMember(x => x.ContactType, o => o.MapFrom(x => ContactType.FromValue(x.ContactType)))
                .ForMember(x => x.PrimaryLanguage, o => o.MapFrom(x => Language.FromValue(x.PrimaryLanguage)))
                .ForMember(x => x.SecondaryLanguage, o => o.MapFrom(x => Language.FromValue(x.SecondaryLanguage)))
                .ForMember(x => x.Gender, o => o.MapFrom(x => Gender.FromValue(x.Gender)));

            CreateMap<UpdateContactForm, UpdateContact>()
                .ForMember(x => x.ContactType, o => o.MapFrom(x => ContactType.FromValue(x.ContactType)))
                .ForMember(x => x.PrimaryLanguage, o => o.MapFrom(x => Language.FromValue(x.PrimaryLanguage)))
                .ForMember(x => x.SecondaryLanguage, o => o.MapFrom(x => Language.FromValue(x.SecondaryLanguage)))
                .ForMember(x => x.Gender, o => o.MapFrom(x => Gender.FromValue(x.Gender)));

            //AssignedContact
            CreateMap<AddAssignedContactForm, AddAssignedContact>()
                .ForMember(x => x.Id, o => o.MapFrom(x => GuidComb.New()))
                .ForMember(x => x.EntityType, o => o.MapFrom(x => EntityType.FromValue(x.EntityTypeValue)));

            CreateMap<UpdateAssignedContactForm, UpdateAssignedContact>()
                .ForMember(x => x.EntityType, o => o.MapFrom(x => EntityType.FromValue(x.EntityTypeValue)));

            //Note
            CreateMap<AddNoteForm, AddNote>()
                .ForMember(x => x.Id, o => o.MapFrom(x => GuidComb.New()))
                .ForMember(x => x.EntityType, o => o.MapFrom(x => EntityType.FromValue(x.EntityTypeValue)));

            CreateMap<UpdateNoteForm, UpdateNote>()
                .ForMember(x => x.EntityType, o => o.MapFrom(x => EntityType.FromValue(x.EntityTypeValue)));

            // Attachment
            CreateMap<AttachmentUploadForm, AddAttachment>()
                .ForMember(x => x.Id, o => o.MapFrom(x => GuidComb.New()))
                .ForMember(x => x.FileData, o => o.Ignore())
                .ForMember(x => x.FileHashCode, o => o.Ignore())
                .ForMember(x => x.EntityType, o => o.MapFrom(x => EntityType.FromValue(x.EntityTypeValue)));

            CreateMap<UpdateAttachmentForm, UpdateAttachment>()
                .ForMember(x => x.Name, o => o.Ignore())
                .ForMember(x => x.ImageData, o => o.Ignore());

            //Lead
            CreateMap<AddLeadForm, AddLead>()
                .ForMember(x => x.Id, o => o.MapFrom(x => GuidComb.New()));

            CreateMap<UpdateLeadForm, UpdateLead>();

            //NewsLetter
            CreateMap<AddNewsLetterForm, AddNewsLetter>()
                .ForMember(x => x.Id, o => o.MapFrom(x => GuidComb.New()));

            CreateMap<UpdateNewsLetterForm, UpdateNewsLetter>();
        }
    }
}