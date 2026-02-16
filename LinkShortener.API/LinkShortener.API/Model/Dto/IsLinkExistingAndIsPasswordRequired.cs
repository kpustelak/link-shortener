namespace LinkShortener.API.Model.DTO;

public class IsLinkExistingAndIsPasswordRequired
{
    public bool IsLinkExisting { get; set; } = false;
    public bool? IsPasswordRequired { get; set; } = false;
}