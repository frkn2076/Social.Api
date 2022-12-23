using Api.Helper;
using Newtonsoft.Json;

namespace Api.Data.Entities;

public class ChatMessage
{
    public int Id { get; set; }

    public string AuthorId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public long CreatedAt { get; set; }

    public string MessageId { get; set; }

    public string Status { get; set; }

    public string Text { get; set; }

    public string Type { get; set; }

    public int ActivityId { get; set; }
}
