using MarketExpress.Core.Entity.Identity;

namespace MarketExpress.Core.Entity
{
    public class UserNotification
    {

        public int Id { get; set; }
        public int NotificationId { get; set; }
        public string UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime ReadAt { get; set; }
        public Notification Notification { get; set; }
        public User User { get; set; }
    }
}
