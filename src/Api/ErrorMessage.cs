using QaCoders_Net.Enums;

namespace QaCoders_Net;

public struct ErrorMessage
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }
    public ErrorType ErrorType { get; set; }

    public ErrorMessage(string errorCode, string message, ErrorType errorType)
    {
        ErrorCode = errorCode;
        Message = message;
        ErrorType = errorType;
    }
}
