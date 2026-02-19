using LinkShortener.API.Interface;
using LinkShortener.API.Model.Dto.Response;

namespace LinkShortener.API.Services;

public class LinkService : ILinkService
{
    public async Task<string> CreateLinkAsync(string url, string shortenedUrl, string? password)
    {
        return shortenedUrl;
    }

    public async Task<IsLinkExistingAndIsPasswordRequiredDtoModel> LinkExistsAsync(string shortenedUrl)
    {
        return new IsLinkExistingAndIsPasswordRequiredDtoModel
        {
            IsLinkExisting = true,
            IsPasswordRequired = true
        };
    }

    public async Task<string?> GetLinkAsync(string shortenedUrl, string? password)
    {
        return "Test";
    }
}