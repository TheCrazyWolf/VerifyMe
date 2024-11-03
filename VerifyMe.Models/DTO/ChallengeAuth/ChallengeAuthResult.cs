namespace VerifyMe.Models.DTO.ChallengeAuth;

public class ChallengeAuthResult
{
    public bool IsSuccess { get; set; }
    public string SystemMessage { get; set; } = string.Empty;
    public DetailsUser? User { get; set; }
    
    public ChallengeAuthResult() {}

    public ChallengeAuthResult(bool isSuccess, string systemMessage, DetailsUser? user = null)
    {
        IsSuccess = isSuccess;
        SystemMessage = systemMessage;
        User = user;
    }
}