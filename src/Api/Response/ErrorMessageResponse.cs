namespace QaCoders_Net.Response;

public class ErrorMessageResponse
{
    public Guid Id { get; set; }
    public string ErrorCode { get; set; }
    public string Message { get; set; }
}
