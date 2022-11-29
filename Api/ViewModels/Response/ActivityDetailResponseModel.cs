namespace Api.ViewModels.Response;

public class ActivityDetailResponseModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Detail { get; set; }

    public string Location { get; set; }

    public DateTime? Date { get; set; }

    public int UserId { get; set; }

    public string PhoneNumber { get; set; }

    public int Capacity { get; set; }

    public string Category { get; set; }

    public List<UserResponseModel> Joiners { get; set; }
}
