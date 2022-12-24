namespace Api.Chat;

public class Message
{
    public Author Author { get; set; }

    public long CreatedAt { get; set; }

    public string Id { get; set; }

    public string Status { get; set; }

    public string Text { get; set; }

    public string Type { get; set; }

    public int Height { get; set; }

    public int Width { get; set; }

    public string Name { get; set; }

    public int Size { get; set; }

    public string Uri { get; set; }
}

public class Author
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}
