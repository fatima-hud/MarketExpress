using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.UserNotification.Request
{
   public class RequestSendNotificationDto
    {
        public int NotificationId {  get; set; }
        public List<string> UserIds {  get; set; }
    }
}
