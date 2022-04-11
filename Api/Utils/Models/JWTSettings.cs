namespace Api.Utils.Models;

public class JWTSettings
{
    public string SecretKey { get; set; }
    
    public int AccessExpireDate { get; set; }
    
    public int RefreshExpireDate { get; set; }
}
