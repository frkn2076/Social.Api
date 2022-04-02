namespace Api.ViewModels;

internal class ActivityDetailViewModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }

    public IEnumerable<string> Participants { get; set; }
}
