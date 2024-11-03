namespace VerifyMe.ApiClient.Models.Resuls.Sms;

public class SmsResultExtended : SmsResult
{
    public string Phone { get; set; } = string.Empty;

    public SmsResultExtended()
    {
        
    }

    public SmsResultExtended(bool isSuccess, string phone, string systemMessage)
    {
        IsSuccess = isSuccess;
        Phone = phone;
        SystemMessage = systemMessage;
    }
}