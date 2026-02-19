namespace LinkShortener.API.Model.Standarized;

public class ResponseModel<T>
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }

    public ResponseModel()
    {
        Status = true;
    }
}