using LinkShortener.API.Interface;
using LinkShortener.API.Model.DTO;

namespace LinkShortener.API.Services;

public class LinkService : ILinkService
{
    public Task<string?> CreateLinkAsync(string url, string userIdentifier, string? password)
    {
        throw new NotImplementedException();
    }

    public Task<IsLinkExistingAndIsPasswordRequired> LinkExistsAsync(string url)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetLinkAsync(string url, string? password)
    {
        throw new NotImplementedException();
    }
}