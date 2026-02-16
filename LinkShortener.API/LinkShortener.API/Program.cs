using LinkShortener.API.Model.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAntiforgery();


builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(configuration);
});



var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAntiforgery();
app.UseHttpsRedirection();

app.MapPost("/add", ([FromBody]CreateLinkDtoModel dto) =>
{
    
    return Results.Ok(dto);
}).DisableAntiforgery();

app.MapGet("/link/{link}/validation", (string link) => $"Hello World! {link}");
app.MapGet("/link/{link}/get", (string link) => $"Hello World! {link}");

app.Run();
