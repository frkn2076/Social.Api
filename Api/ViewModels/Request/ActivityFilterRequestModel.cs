namespace Api.ViewModels.Request;

public class ActivityFilterRequestModel
{
    public string? Key { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public int FromCapacity { get; set; }

    public int ToCapacity { get; set; }
}
