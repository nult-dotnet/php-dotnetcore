using Microsoft.AspNetCore.SignalR;

namespace BookStoreApi.SignalR
{
    public class BroadcastHub : Hub<IHubClient>
    {
        public async Task SendMessage()
       => await Clients.All.BroadcastMessage();
    }
}