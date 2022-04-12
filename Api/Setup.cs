using Api.Data;
using Api.Data.Contracts;
using Api.Data.Entities;
using Api.Data.Implementations;
using Api.Data.Repositories.Contracts;
using Api.Data.Repositories.Implementations;
using Api.Enums;
using Api.Service.Contracts;
using Api.Service.Implementations;
using Api.Utils;
using Api.Utils.Models;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;

namespace Api;

public static class Setup
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpContextAccessor();

        services.AddSession(option =>
        {
            option.IdleTimeout = TimeSpan.FromMinutes(1);
        });

        services.AddDistributedMemoryCache();

        services.Configure<JWTSettings>(builder.Configuration.GetSection(nameof(JWTSettings)));
        services.Configure<AdminCredentials>(builder.Configuration.GetSection(nameof(AdminCredentials)));

        services.AddTransient<IConnectionService, ConnectionService>();
        services.AddTransient<ISocialRepository, SocialRepository>();

        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IProfileService, ProfileService>();

        services.AddScoped<CurrentUser>();

        var jwtSettings = new JWTSettings();
        builder.Configuration.GetSection(nameof(JWTSettings)).Bind(jwtSettings);
        services.RegisterJWTAuthorization(jwtSettings);
    }

    public static async void CreateSchemes(this IDbConnection dbConnection)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var folderPath = Path.Combine(currentDirectory, Queries.SchemeFolderPath);
        var schemeQueryFileNames = Directory.GetFiles(folderPath);

        using var transaction = dbConnection.BeginTransaction();
        foreach (var schemeQueryFileName in schemeQueryFileNames)
        {
            var schemeQuery = FileResourceUtility.LoadResource(folderPath, schemeQueryFileName);
            await dbConnection.ExecuteAsync(schemeQuery, transaction: transaction);
        }
        transaction.Commit();
    }

    public static async Task TempAdminCredentialsRegisterAsync(this ISocialRepository socialRepository, AdminCredentials adminCredentials)
    {
        var admin = new Profile()
        {
            UserName = adminCredentials.UserName,
            Password = adminCredentials.EncryptedPassword,
            Role = Roles.Admin
        };

        await socialRepository.CreateProfileAsync(admin);
    }

    #region Helper

    private static void RegisterJWTAuthorization(this IServiceCollection services, JWTSettings settings)
    {
        var key = Encoding.ASCII.GetBytes(settings.SecretKey);
        services.AddAuthentication(x => {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x => {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }

    private static T GetOptions<T>(this WebApplicationBuilder builder) where T : new()
    {
        var t = new T();
        builder.Configuration.GetSection(nameof(T)).Bind(t);
        return t;
    }

    #endregion Helper
}
