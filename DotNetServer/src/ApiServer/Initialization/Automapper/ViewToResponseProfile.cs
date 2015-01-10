using AutoMapper;
using Common.Base;
using Common.SystemSettings;
using Core.ViewOnly.Base;
using Core.Views;
using Dto.ApiResponses;
using Dto.ApiResponses.AppUserResponses;
using Dto.ApiResponses.AttachmentResponses;
using Dto.ApiResponses.ConfigResponses;
using Dto.ApiResponses.ContactResponses;
using Dto.ApiResponses.HealthResponses;
using Dto.ApiResponses.LeadResponses;
using Dto.ApiResponses.NoteResponses;
using Dto.ApiResponses.TaskLogResponses;
using Dto.ApiResponses.TemplateResponses;

namespace WebApp.Initialization.Automapper
{
    public class ViewToResponseProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.AddGlobalIgnore("Uri");
            Mapper.AddGlobalIgnore("RedirectRequired");
            Mapper.AddGlobalIgnore("ValidationObjects");

            // member
            CreateMap<AppUserView, AppUserResponse>();

            CreateMap<AppUserView, AppUserLine>()
                .ForMember(x => x.UserStatusName, o => o.Ignore());

            CreateMap<Page<AppUserView>, PageResponse<AppUserLine>>();

            // health
            CreateMap<HealthView, HealthResponse.HealthLine>();

            CreateMap<EmailConfig, EmailConfigResponse>();

            // configuration
            CreateMap<SmsConfig, SmsConfigResponse>();

            CreateMap<NetworkConfig, NetworkConfigResponse>();

            // template
            CreateMap<TemplateView, TemplateLine>();

            CreateMap<TemplateView, TemplateResponse>();

            CreateMap<Page<TemplateView>, PageResponse<TemplateLine>>();

            // lead
            CreateMap<LeadView, LeadLine>();

            CreateMap<LeadView, LeadResponse>();

            CreateMap<Page<LeadView>, PageResponse<LeadLine>>();

            // contact
            CreateMap<ContactView, ContactLine>();

            CreateMap<ContactView, ContactResponse>();

            CreateMap<Page<ContactView>, PageResponse<ContactLine>>();

            // assignedContact
            CreateMap<AssignedContactView, AssignedContactLine>();

            CreateMap<AssignedContactView, AssignedContactResponse>();

            CreateMap<Page<AssignedContactView>, PageResponse<AssignedContactLine>>();

            // memberview to guid-name pair
            CreateMap<AppUserView, GuidNamePair>();

            // note
            CreateMap<NoteView, NoteLine>();

            // attachment
            CreateMap<AttachmentView, AttachmentLine>();

            // taskLog
            CreateMap<TaskLogView, TaskLogLine>();

            CreateMap<TaskLogView, TaskLogResponse>();

            CreateMap<Page<TaskLogView>, PageResponse<TaskLogLine>>();

        }
    }
}