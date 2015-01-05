using System;
using System.Collections.Generic;

namespace NSBus.Dto.Commands
{
    public class SendNotificationCommand
    {
        public Guid UserId { get; set; }
        public string NotificationTypeValue { get; set; }

        public Dictionary<string, object> StaticData { get; set; }
        public Dictionary<string, object> ViewDataSingle { get; set; }
    }
}
