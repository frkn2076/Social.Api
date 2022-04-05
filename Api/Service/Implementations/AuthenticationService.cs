using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Api.Service.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace Api.Service.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly ISocialRepository _socialRepository;

    public AuthenticationService(ISocialRepository socialRepository)
    {
        _socialRepository = socialRepository;
    }

    public async Task Register(string email, string userPassword)
    {
        var encryptedUserPassword = EncryptPassword(userPassword);
        var profile = new Profile()
        {
            Email = email,
            Password = encryptedUserPassword,
        };

        await _socialRepository.CreateProfileAsync(profile);
    }

    public async Task<bool> ValidatePassword(string email, string userPassword)
    {
        var password = await _socialRepository.GetPasswordAsync(email);
        var encryptedUserPassword = EncryptPassword(userPassword);
        return password == encryptedUserPassword;
    }

    #region Helper
    private string EncryptPassword(string password)
    {
        var md5 = new MD5CryptoServiceProvider();
        md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
        var result = md5.Hash.ToList();

        var strBuilder = new StringBuilder();
        result.ForEach(x => strBuilder.Append(x.ToString("x2")));
        return strBuilder.ToString();
    }

    #endregion Helper
}
