﻿using Api.Data;
using Api.Data.Contracts;
using Api.Data.Implementations;
using Api.Data.Repositories.Contracts;
using Api.Data.Repositories.Implementations;
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
        services.RegisterJWTAuthorization();

        services.Configure<JWTSettings>(builder.Configuration.GetSection(nameof(JWTSettings)));

        var jwtSettings = new JWTSettings();
        builder.Configuration.GetSection(nameof(JWTSettings)).Bind(jwtSettings);

        services.AddTransient<IConnectionService, ConnectionService>();
        services.AddTransient<ISocialRepository, SocialRepository>();

        services.AddTransient<IAuthenticationService, AuthenticationService>();

        services.AddScoped<CurrentUser>();
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

    private static void RegisterJWTAuthorization(this IServiceCollection services)
    {
        var key = Encoding.ASCII.GetBytes("SECRET KEY SECRET KEY SECRET KEY SECRET KEY");
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
}
