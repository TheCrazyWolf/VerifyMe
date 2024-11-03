using Telegram.Bot;
using VerifyMe.Models.DLA;
using VerifyMe.Models.DTO.ChallengeAuth;
using VerifyMe.Models.DTO.Sms;
using VerifyMe.Models.Enums;
using VerifyMe.Services.Extensions;
using VerifyMe.Storage;

namespace VerifyMe.Services.AuthServices;

public class AuthService(VerifyStorage storage)
{
    public async Task<User?> GetUserByPhoneNumberAsync(string dtoPhone)
    {
        return await storage.Users.GetUserByPhoneAsync(dtoPhone.GetNormalizedPhoneNumber());
    }

    public async Task<ChallengeAuth> CreateChallengeAuthAsync(App application, User user)
    {
        var challenge = new ChallengeAuth()
        {
            UserId = user.Id,
            ApplicationId = application.Id,
            Created = DateTime.Now,
            Status = ChallengeStatus.Unknown,
        };
        
        await storage.ChallengesAuths.CreateChallengeAsync(challenge);
        return challenge;
    }

    public async Task RejectInActiveChallengesAsync()
    {
        var challenges = await storage.ChallengesAuths.GetChallengesWithUnknownStatusAsync();

        foreach (var challenge in challenges)
        {
            challenge.Status = ChallengeStatus.Rejected;
            await storage.ChallengesAuths.UpdateChallengeAsync(challenge);
        }
    }

    public async Task<ChallengeAuthResult> WaitResultOfChallengeAsync(ChallengeAuth challengeAuth, int attempts)
    {
        for (int i = 0; i < attempts; i++)
        {
            var actualChallenge = await storage.ChallengesAuths.GetChallengeByIdAsync(challengeAuth.Id);
            if(actualChallenge is null) return new ChallengeAuthResult(false, $"ChallengeId {challengeAuth.Id} not found");

            switch (actualChallenge.Status)
            {
                case ChallengeStatus.Accept when actualChallenge.User is not null:
                    return new ChallengeAuthResult(true, "Успешная авторизация", 
                        new DetailsUser(telegramId:actualChallenge.User.Id, username: actualChallenge.User.UserName, firstName: actualChallenge.User.FirstName, lastName: actualChallenge.User.LastName,
                            phone: actualChallenge.User.PhoneNumber));
                case ChallengeStatus.Rejected:
                    return new ChallengeAuthResult(false, "Пользователь не принял авторизацию");
                default:
                    await Task.Delay(1000);
                    break;
            }
        }
        
        return new ChallengeAuthResult(false, "Пользователь не принял авторизацию");
    }

    public async Task<ChallengeAuthResult> UpdateChallengeFromCallbackDataAsync(string challengeId,
        ChallengeStatus newStatus)
    {
        await RejectInActiveChallengesAsync();
        var challenge = await storage.ChallengesAuths.GetChallengeByIdAsync(challengeId);
        if (challenge is null) return new ChallengeAuthResult(false, $"ChallengeId #({challengeId}) not found");
        if(challenge.Status is ChallengeStatus.Accept or ChallengeStatus.Rejected) return new ChallengeAuthResult(false, "⚠️ Время подтверждения истекло"); 
        challenge.Status = newStatus;
        await storage.ChallengesAuths.UpdateChallengeAsync(challenge);
        return new ChallengeAuthResult(true, newStatus is ChallengeStatus.Accept ? $"✅ Успешная авторизация в сервисе: <b>{challenge.Application?.Name}</b>" : $"⚠️ Запрос на авторизацию отклонен в сервисе: <b>{challenge.Application?.Name} </b>");
    }
}