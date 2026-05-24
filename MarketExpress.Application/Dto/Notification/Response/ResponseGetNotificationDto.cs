using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Notification.Response
{
   public class ResponseGetNotificationDto
    {
        public int NotificationId { get; set; }
        public string Title { get; set; }

        public string Message { get; set; }
        public enNotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public bool IsGlobal { get; set; }
        public bool IsRead { get; set; }
        public DateTime ReadAt { get; set; }
        public int? StoreId { get; set; }
        public string? StoreName { get; set; }
    }
}
