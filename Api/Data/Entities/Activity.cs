namespace Api.Data.Entities;

public class Activity
{
    public int Id { get; set; }

    public string Title { get; set; }
    
    public string Detail { get; set; }

    public string Location { get; set; }
    
    public DateTime? Date { get; set; }

    public string PhoneNumber { get; set; }

    public int OwnerProfileId { get; set; }

    public int Capacity { get; set; }

    public string Category { get; set; }
}
