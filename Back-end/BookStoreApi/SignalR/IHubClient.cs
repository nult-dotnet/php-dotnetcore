namespace BookStoreApi.SignalR
{
    public interface IHubClient
    {
        Task BroadcastMessage();
    }
}