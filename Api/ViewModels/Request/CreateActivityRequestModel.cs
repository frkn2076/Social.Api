namespace Api.ViewModels.Request;

public class CreateActivityRequestModel
{
    public string Title { get; set; }

    public string Detail { get; set; }

    public DateTime? Date { get; set; }

    public string? Location { get; set; }

    public string? PhoneNumber { get; set; }

    public int Capacity { get; set; }
}
