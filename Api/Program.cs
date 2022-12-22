using Api;
using Api.Chat;
using Api.Middleware;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        Console.WriteLine(exception);

        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        //await context.Response.WriteAsJsonAsync(response);

        await Task.CompletedTask;
    });
});

app.UseHttpsRedirection();

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.UseMiddleware<AuthenticationMiddlewareForRefreshToken>();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.UseHealthChecks("/health");

app.Run();
