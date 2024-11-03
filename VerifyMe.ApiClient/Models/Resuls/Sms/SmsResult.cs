namespace VerifyMe.ApiClient.Models.Resuls.Sms;

public class SmsResult
{
    public bool IsSuccess { get; set; }
    public string SystemMessage { get; set; } = string.Empty;

    public SmsResult(bool isSuccess, string systemMessage)
    {
        IsSuccess = isSuccess;
        SystemMessage = systemMessage;
    }

    public SmsResult()
    {
        
    }
}