namespace VerifyMe.Models.DTO;

public class PostSendSms
{
    public string Phone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}