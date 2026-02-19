namespace LinkShortener.API.Model.Dto.Response;

public class IsLinkExistingAndIsPasswordRequiredDtoModel
{
    public bool IsLinkExisting { get; set; } = false;
    public bool? IsPasswordRequired { get; set; } = false;
}