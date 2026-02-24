namespace LinkShortener.API.Model.Entities;

public class LinkModel
{
    public required string Url { get; set; }
    public string? Password { get; set; } = string.Empty;
}