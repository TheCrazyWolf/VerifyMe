namespace VerifyMe.ApiClient.Models.Post.Sms;

public class SendSms
{
    public string Phone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    
    public SendSms() { }

    public SendSms(string phone, string message)
    {
        Phone = phone;
        Message = message;
    }
}