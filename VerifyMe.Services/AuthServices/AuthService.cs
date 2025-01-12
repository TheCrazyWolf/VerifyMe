using VerifyMe.Models.DLA;
using VerifyMe.Models.DTO.ChallengeAuth;
using VerifyMe.Models.Enums;
using VerifyMe.Services.Extensions;
using VerifyMe.Storage;

namespace VerifyMe.Services.AuthServices;

public class AuthService(VerifyStorage storage)
{
    private const int DefaultLifeChallengeInSeconds = 60;
    private readonly List<ChallengeAuth> _challengeAuths = [];
    
    public async Task<User?> GetUserByPhoneNumberAsync(string dtoPhone)
    {
        return await storage.Users.GetUserByPhoneAsync(phone: dtoPhone.GetNormalizedPhoneNumber());
    }

    public ChallengeAuth CreateChallengeAuth(App application, User user)
    {
        var challenge = new ChallengeAuth
        {
            Id = Guid.NewGuid().ToString().Split("-").First(),
            UserId = user.Id,
            ApplicationId = application.Id,
            Created = DateTime.Now,
            Status = ChallengeStatus.Unknown,
        };
        
        _challengeAuths.Add(challenge);
        return challenge;
    }

    public void RejectInActiveChallenges()
    {
        foreach (var challenge in _challengeAuths
                     .Where(x => x.Status == ChallengeStatus.Unknown)
                     .Where(x => DateTime.Now >= x.Created.AddSeconds(DefaultLifeChallengeInSeconds)).ToList())
        {
            challenge.Status = ChallengeStatus.Rejected;
        }
    }

    public async Task<ChallengeAuthResult> WaitResultOfChallengeAsync(ChallengeAuth challengeAuth, int attempts)
    {
        for (int i = 0; i < attempts; i++)
        {
            var actualChallenge = _challengeAuths.FirstOrDefault(x=> x.Id == challengeAuth.Id);
            if(actualChallenge is null) return new ChallengeAuthResult(false, $"ChallengeId {challengeAuth.Id} not found");

            switch (actualChallenge.Status)
            {
                case ChallengeStatus.Accept when actualChallenge.User is not null:
                    return new ChallengeAuthResult(true, "Успешная авторизация", 
                        new DetailsUser(telegramId:actualChallenge.User.Id, username: actualChallenge.User.UserName, firstName: actualChallenge.User.FirstName, lastName: actualChallenge.User.LastName, phone: actualChallenge.User.PhoneNumber));
                case ChallengeStatus.Rejected:
                    return new ChallengeAuthResult(false, "Пользователь не принял авторизацию");
                default:
                    await Task.Delay(1000);
                    break;
            }
        }
        
        return new ChallengeAuthResult(false, "Пользователь не принял авторизацию");
    }

    public ChallengeAuthResult UpdateChallengeFromCallbackDataAsync(string challengeId, ChallengeStatus newStatus)
    {
        RejectInActiveChallenges();
        var challenge = _challengeAuths.FirstOrDefault(x => x.Id == challengeId);
        if (challenge is null) return new ChallengeAuthResult(false, $"ChallengeId {challengeId} not found");
        if(challenge.Status is ChallengeStatus.Accept or ChallengeStatus.Rejected) return new ChallengeAuthResult(false, "⚠️ Время подтверждения истекло"); 
        challenge.Status = newStatus;
        return new ChallengeAuthResult(true, newStatus is ChallengeStatus.Accept ? $"✅ Успешная авторизация в сервисе: <b>{challenge.Application?.Name}</b>" : $"⚠️ Запрос на авторизацию отклонен в сервисе: <b>{challenge.Application?.Name} </b>");
    }
}