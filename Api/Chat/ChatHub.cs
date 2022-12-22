using Api.Data.NoSql.Contracts;
using Api.Data.Repositories.Contracts;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

namespace Api.Chat;

public class ChatHub : Hub
{
    private readonly ISocialRepository _socialRepository;
    private readonly IMongoDBRepository _mongoDBRepository;

    public ChatHub(ISocialRepository socialRepository, IMongoDBRepository mongoDBRepository)
    {
        _socialRepository = socialRepository;
        _mongoDBRepository = mongoDBRepository;
    }

    public async Task GroupSendMessage(string message)
    {
        try
        {
            var roomId = JObject.Parse(message)["id"].ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await _mongoDBRepository.InsertMessage(message);
            await Clients.Group(roomId).SendAsync("GroupSendMessage", message);
        }
        catch (Exception e)
        {

            throw;
        }
        
    }
}
