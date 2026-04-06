using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
    public class Notification
    {
        public int Id {  get; set; }
        public string Title {  get; set; }
        
        public string Message {  get; set; }
        public enNotificationType Type { get; set; }
        public DateTime CreatedAt {  get; set; }
        public string CreatedBy { get; set; }
        public bool IsGlobal { get; set; }
        public ICollection<UserNotification> UserNotifications {  get; set; }
        public User OwnerStore { get; set; }
       

    }
}
