using Api;
using Api.Data.Contracts;
using Api.Data.Repositories.Contracts;
using Api.Utils.Models;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var serviceProvider = builder.Services.BuildServiceProvider();
var connectionService = serviceProvider.GetRequiredService<IConnectionService>();
connectionService.GetPostgresConnection().CreateSchemes();

var adminCredentials = new AdminCredentials();
builder.Configuration.GetSection(nameof(AdminCredentials)).Bind(adminCredentials);
var socialRepository = serviceProvider.GetRequiredService<ISocialRepository>();
await socialRepository.TempAdminCredentialsRegisterAsync(adminCredentials);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseSession();

app.Run();
