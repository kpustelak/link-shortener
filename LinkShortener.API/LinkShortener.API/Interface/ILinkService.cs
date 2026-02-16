using LinkShortener.API.Model.DTO;

namespace LinkShortener.API.Interface;

public interface ILinkService
{
    public Task<string?> CreateLinkAsync(string url, string userIdentifier, string? password );
    public Task<IsLinkExistingAndIsPasswordRequired> LinkExistsAsync(string url);
    public Task<string?> GetLinkAsync(string url, string? password);
}