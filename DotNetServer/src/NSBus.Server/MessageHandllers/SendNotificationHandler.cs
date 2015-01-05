using System.Collections.Generic;
using System.Dynamic;
using Common.Enumerations;
using Common.Helpers;
using Common.Service;
using Core.ViewOnly;
using Core.Views;
using NSBus.Dto.Commands;
using NServiceBus;
using NServiceBus.Logging;
using SmartFormat;

namespace NSBus.Server.MessageHandllers
{
    public class SendNotificationHandler : IHandleMessages<SendNotificationCommand>
    {

        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IViewRepository<AppUserView> _appUserViewRepository;
        private readonly IViewRepository<TemplateView> _templateViewRepository;

        private readonly ILog _log = LogManager.GetLogger(typeof(MessageHandler<>));

        public SendNotificationHandler(IEmailSender emailSender, ISmsSender smsSender, IViewRepository<AppUserView> appUserViewRepository, IViewRepository<TemplateView> templateViewRepository)
        {
            _emailSender = emailSender;
            _smsSender = smsSender;
            _appUserViewRepository = appUserViewRepository;
            _templateViewRepository = templateViewRepository;
        }

        public void Handle(SendNotificationCommand message)
        {
            _log.InfoFormat("Got command : {0}", message.ToString());

            var appUser = _appUserViewRepository.GetById(message.UserId);

            var templateName = NotificationType.FromValue(message.NotificationTypeValue).DisplayName;

            var template = _templateViewRepository.GetByKey(Property.Of<TemplateView>(x => x.Name), templateName);

            dynamic data = new ExpandoObject();

            var expando = (IDictionary<string, object>)data;
            expando["User"] = appUser;

            if(message.StaticData == null) message.StaticData = new Dictionary<string, object>();
            if(message.ViewDataSingle == null) message.ViewDataSingle = new Dictionary<string, object>();

            foreach (var k in message.StaticData.Keys)
            {
                expando[k] = message.StaticData[k];
            }

            foreach (var s in message.ViewDataSingle.Keys)
            {
                expando[s] = message.ViewDataSingle[s];
            }

            var specificCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-us");
            var mailBody = Smart.Format(specificCulture, template.MailBody, data);
            _log.Info(mailBody);
            _emailSender.SendTextEmail("Notification Mail", mailBody, appUser.Email);
            
            var smsBody = Smart.Format(specificCulture, template.SmsBody, data);
            _log.Info(smsBody);
            _smsSender.SendShortMessage(smsBody, appUser.Mobile);
        }
    }
}
