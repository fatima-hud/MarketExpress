using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Notification.Request
{
   public class RequestAddNotificationDto
    {
        public string Title { get; set; }

        public string Message { get; set; }
      
        public enNotificationType Type { get; set; }
        public List<string>? UserIds { get; set; }
        public bool IsGlobal {  get; set; }
    }
}
