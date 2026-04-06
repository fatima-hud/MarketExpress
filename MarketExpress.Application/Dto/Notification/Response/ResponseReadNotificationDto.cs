namespace MarketExpress.Application.Dto.Notification.Response
{
    public class ResponseReadNotificationDto
    {
        public int NotificationId { get; set; }
        public bool IsRead { get; set; }
        public DateTime ReadAt { get; set; }
    }
}
