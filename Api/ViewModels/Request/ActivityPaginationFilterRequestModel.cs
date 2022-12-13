namespace Api.ViewModels.Request;

public class ActivityPaginationFilterRequestModel
{
    public bool IsRefresh { get; set; }

    public string? Key { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public int FromCapacity { get; set; }

    public int ToCapacity { get; set; }

    public List<string> Categories { get; set; }
}
