namespace VerifyMe.Models.DTO;

public class SmsResult
{
    public bool IsSuccess { get; set; }
    public string SystemMessage { get; set; } = string.Empty;
}