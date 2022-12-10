using Api.Data.Repositories.Contracts;
using Api.Data.Repositories.Implementations;
using Api.Extensions;
using Api.Service.Contracts;
using Api.Service.Implementations;
using Api.Utils.Models;
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

        services.AddHealthChecks();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpContextAccessor();

        services.AddSession(x => x.IdleTimeout = TimeSpan.FromHours(1));

        services.AddDistributedMemoryCache();

        services.Configure<JWTSettings>(builder.Configuration.GetSection(nameof(JWTSettings)));
        services.Configure<AdminCredentials>(builder.Configuration.GetSection(nameof(AdminCredentials)));

        services.AddTransient<ISocialRepository, SocialRepository>();

        services.AddTransient<IActivityService, ActivityService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IProfileService, ProfileService>();

        services.AddScoped<CurrentUser>();

        var jwtSettings = builder.Configuration.GetOptions<JWTSettings>();
        services.RegisterJWTAuthentication(jwtSettings);

        services.AddCors();
    }

    #region Helper

    private static void RegisterJWTAuthentication(this IServiceCollection services, JWTSettings settings)
    {
        var key = Encoding.ASCII.GetBytes(settings.SecretKey);
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
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
