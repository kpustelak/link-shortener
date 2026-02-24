using LinkShortener.API.Extensions;
using LinkShortener.API.Interface;
using LinkShortener.API.Model.DTO;
using LinkShortener.API.Model.Standarized;
using Microsoft.AspNetCore.Mvc;
using LinkShortener.API.Model.Dto.Response;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAntiforgery();

builder.Services.AddRedisDatabase(builder.Configuration);
builder.Services.AddLinkServices();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAntiforgery();
app.UseHttpsRedirection();

# region Minimal api endpoint functions

AddLink();
GetLinkInfo();
GetLink();

# endregion

app.Run();

void AddLink()
{
    app.MapPost("/add", async ([FromBody]CreateLinkDtoModel request,  IConfiguration conf, ILinkService linkService) =>
    {
        ResponseModel<string> result = new ResponseModel<string>();
    
        if (conf.GetSection("userIdentifier").Value == request.UserIdentifier)
        {
            try
            {
                result.Data = await linkService.CreateLinkAsync(request.Url, request.ShortenedUrl,request.Password); 
                result.Message = "Link created successfully";
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "There was an error creating link";
                result.Errors.Add(ex.Message);
                return Results.BadRequest(result);
            
            }
        }
    
        result.Status = false;
        result.Message = "You identifier is not valid. ";
        return Results.BadRequest(result);

    }).DisableAntiforgery();
}
void GetLinkInfo()
{
    app.MapGet("/link/{shortenedUrl}/validation", async ([FromRoute]string shortenedUrl, ILinkService linkService) =>
    {
        ResponseModel<IsLinkExistingAndIsPasswordRequiredDtoModel> result = 
            new ResponseModel<IsLinkExistingAndIsPasswordRequiredDtoModel>();
    
        try
        {
            result.Data = await linkService.LinkExistsAsync(shortenedUrl);
            result.Message = "Link exists";
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            result.Status = false;
            result.Message = "There was an error validating link";
            result.Errors.Add(ex.Message);
            return Results.BadRequest(result);
        }
    
    }).DisableAntiforgery();
}
void GetLink()
{
    app.MapPost("/link/{shortenedUrl}/get", async ([FromRoute]string shortenedUrl, [FromBody]GetUrlDtoModel? request, ILinkService linkService) =>
    {
        ResponseModel<string> result = new ResponseModel<string>();
        try
        {
            var originalUrl = await linkService.GetLinkAsync(shortenedUrl, request?.Password);
            if (originalUrl == null)
            {
                result.Status = false;
                result.Message = "Shortened url not found or wrong password.";
                return Results.BadRequest(result);
            }
        
            result.Data = originalUrl;
            result.Message = "Link found";
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            result.Status = false;
            result.Message = "There was an error getting link";
            result.Errors = [];
            result.Errors.Add(ex.Message);
            return Results.BadRequest(result);
        }
    }).DisableAntiforgery();
}