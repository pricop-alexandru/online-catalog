using Microsoft.AspNetCore.SignalR;

namespace Catalog_Online_Mitica_Pricop_Vasii.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessageToProfessor(string professorId, string message)
        {
            await Clients.User(professorId).SendAsync("ReceiveMessage", message);
        }
    }
}