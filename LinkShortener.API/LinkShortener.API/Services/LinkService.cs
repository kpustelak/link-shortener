using LinkShortener.API.Interface;
using LinkShortener.API.Model.Dto.Response;
using LinkShortener.API.Model.Entities;

namespace LinkShortener.API.Services;

public class LinkService : ILinkService
{
    private readonly IRedisCacheService  _redisCacheService;
    private readonly IPasswordService _passwordService;
    
    public LinkService(IRedisCacheService redisCacheService , IPasswordService passwordService)
    {
        _redisCacheService = redisCacheService;
        _passwordService = passwordService;
    }
    
    public async Task<string> CreateLinkAsync(string url, string shortenedUrl, string? password)
    {
        string key = shortenedUrl;
        var data = new LinkModel{Url = url, Password = password == null ? null :  _passwordService.HashPassword(password)};
        if (await _redisCacheService.GetAsync<LinkModel>(key) != null)
        {
            throw new Exception("This link already exists. Try other one.");
        }
        await _redisCacheService.SetAsync(key, data, new TimeSpan(24, 0, 0));
        return key;
    }

    public async Task<IsLinkExistingAndIsPasswordRequiredDtoModel> LinkExistsAsync(string shortenedUrl)
    {
        var data = await _redisCacheService.GetAsync<LinkModel>(shortenedUrl);
        return new IsLinkExistingAndIsPasswordRequiredDtoModel
        {
            IsLinkExisting = data != null ? true : false,
            IsPasswordRequired = data != null && data.Password != null ?  true : false
        };
    }

    public async Task<string?> GetLinkAsync(string shortenedUrl, string? password)
    {
        var data = await _redisCacheService.GetAsync<LinkModel>(shortenedUrl);
        
        if (data == null) { throw new Exception("This link does not exists."); }
        
        
        if (data.Password == null || _passwordService.VerifyHashedPassword(data.Password, password ?? string.Empty)) { return data.Url; }
        
        throw new Exception("This password does not match.");
    }
}