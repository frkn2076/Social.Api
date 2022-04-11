using Api;
using Api.Data.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var serviceProvider = builder.Services.BuildServiceProvider();
var connectionService = serviceProvider.GetRequiredService<IConnectionService>();
connectionService.GetPostgresConnection().CreateSchemes();

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

app.Run();
