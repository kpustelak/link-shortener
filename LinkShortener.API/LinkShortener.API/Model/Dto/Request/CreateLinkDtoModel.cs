namespace LinkShortener.API.Model.DTO;

public class CreateLinkDtoModel
{
    public required string Url { get; set; }
    public string ShortenedUrl { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public required string UserIdentifier { get; set; }
}