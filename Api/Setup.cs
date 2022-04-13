using Api.Data;
using Api.Data.Contracts;
using Api.Data.Entities;
using Api.Data.Implementations;
using Api.Data.Repositories.Contracts;
using Api.Data.Repositories.Implementations;
using Api.Enums;
using Api.Extensions;
using Api.Service.Contracts;
using Api.Service.Implementations;
using Api.Utils;
using Api.Utils.Models;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api;

public static class Setup
{
    public static async void ConfigureServices(this WebApplicationBuilder builder)
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

        var jwtSettings = builder.Configuration.GetOptions<JWTSettings>();
        services.RegisterJWTAuthorization(jwtSettings);

        var serviceProvider = services.BuildServiceProvider();

        await serviceProvider.CreateSchemesAsync();

        var adminCredentials = builder.Configuration.GetOptions<AdminCredentials>();
        await serviceProvider.TempAdminCredentialsRegisterAsync(adminCredentials);
    }

    #region Helper

    private static async Task CreateSchemesAsync(this ServiceProvider serviceProvider)
    {
        
        var connectionService = serviceProvider.GetRequiredService<IConnectionService>();
        var dbConnection = connectionService.GetPostgresConnection();

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

    private static async Task TempAdminCredentialsRegisterAsync(this ServiceProvider serviceProvider, AdminCredentials adminCredentials)
    {
        var socialRepository = serviceProvider.GetRequiredService<ISocialRepository>();

        var admin = new Profile()
        {
            UserName = adminCredentials.UserName,
            Password = adminCredentials.EncryptedPassword,
            Role = Roles.Admin
        };

        var profile = await socialRepository.GetProfileByUserNameAsync(adminCredentials.UserName);

        if (profile != null)
        {
            return;
        }

        await socialRepository.CreateProfileAsync(admin);
    }

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

    #endregion Helper
}
