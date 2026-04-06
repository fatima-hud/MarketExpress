using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
using MarketExpress.Database;
using Microsoft.AspNetCore.SignalR;

namespace MarketExpress.Hubs
{
    public class ChatHub:Hub
    {
        private readonly ApplicationDbContext _dbContext;
        public ChatHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task SendMessage(int complaintId, string userId, string message)
        {
            var newMessage = new Message
            {
                ComplaintId = complaintId,
                SenderId = userId,
                MessageText = message,
                SentAt = DateTime.UtcNow
            };

            _dbContext.Messages.Add(newMessage);

            await _dbContext.SaveChangesAsync();

            await Clients.Group(complaintId.ToString())
                .SendAsync(
                    "ReceiveMessage",
                    userId,
                    message,
                    newMessage.Id// ✅ ID الحقيقي من DB
                );
        }
        public async Task JoinComplaint(int complaintId)
        {
            try
            {

                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    complaintId.ToString()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                throw;
            }

        }

    }
}

    

