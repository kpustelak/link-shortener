using LinkShortener.API.Model.Dto.Response;

namespace LinkShortener.API.Interface;

public interface ILinkService
{
    public Task<string> CreateLinkAsync(string url, string shortenedUrl,string? password );
    public Task<IsLinkExistingAndIsPasswordRequiredDtoModel> LinkExistsAsync(string shortenedUrl);
    public Task<string?> GetLinkAsync(string shortenedUrl, string? password);
}