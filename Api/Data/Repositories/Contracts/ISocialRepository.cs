using Api.Data.Entities;
using System.Data;

namespace Api.Data.Repositories.Contracts;

public interface ISocialRepository
{
    Task CreateProfileAsync(Profile profile, IDbTransaction transaction = null);
}
