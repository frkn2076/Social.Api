using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Api.Chat;

public class ChatHub : Hub
{
    private readonly ISocialRepository _socialRepository;

    public ChatHub(ISocialRepository socialRepository)
    {
        _socialRepository = socialRepository;
    }

    public async Task JoinGroup(string activityId)
    {
        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
        }
        catch (Exception e)
        {
            // log here
            throw;
        }
    }

    public async Task GroupSendMessage(string message)
    {
        try
        {
            var convertedMessage = JsonConvert.DeserializeObject<Message>(message);

            ChatMessage chatMessage = new ChatMessage()
            {
                AuthorId = convertedMessage.Author.Id,
                FirstName = convertedMessage.Author.FirstName,
                LastName = convertedMessage.Author.LastName,
                MessageId = convertedMessage.Id,
                CreatedAt = convertedMessage.CreatedAt,
                ActivityId = Convert.ToInt32(convertedMessage.Id),
                Status = convertedMessage.Status,
                Text = convertedMessage.Text,
                Type = convertedMessage.Type
            };

            await _socialRepository.CreateChatMessageAsync(chatMessage);
            await Clients.GroupExcept(convertedMessage.Id, Context.ConnectionId).SendAsync("GroupSendMessage", message);
        }
        catch (Exception e)
        {
            //log here
            throw;
        }

    }
}
