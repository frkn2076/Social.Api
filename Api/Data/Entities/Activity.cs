namespace Api.Data.Entities;

public class Activity
{
    public int Id { get; set; }

    public string Title { get; set; }
    
    public string Detail { get; set; }

    public string Location { get; set; }
    
    public DateTime Date { get; set; }
}
