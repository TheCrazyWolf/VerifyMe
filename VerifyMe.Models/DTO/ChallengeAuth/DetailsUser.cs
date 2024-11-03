namespace VerifyMe.Models.DTO.ChallengeAuth;

public class DetailsUser
{
    public long TelegramId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public DetailsUser()
    {
        
    }

    public DetailsUser(long telegramId, string username, string firstName, string lastName, string phone)
    {
        TelegramId = telegramId;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
    }
}