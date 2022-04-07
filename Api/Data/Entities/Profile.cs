namespace Api.Data.Entities;

public class Profile
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
    
    public string Photo { get; set; }

    public string RefreshToken { get; set; }

    public DateTime ExpireDate { get; set; }
}
