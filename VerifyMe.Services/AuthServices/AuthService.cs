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
    public async Task<User?> GetUserByPhoneNumber(string dtoPhone)
    {
        return await storage.Users.GetUserByPhone(dtoPhone.GetNormalizedPhoneNumber());
    }

    public async Task<ChallengeAuth> CreateChallengeAuth(App application, User user)
    {
        var challenge = new ChallengeAuth()
        {
            UserId = user.Id,
            ApplicationId = application.Id,
            Created = DateTime.Now,
            Status = ChallengeStatus.Unknown,
        };
        
        await storage.ChallengesAuths.CreateChallenge(challenge);
        return challenge;
    }

    public async Task RejectInActiveChallenges()
    {
        var challenges = await storage.ChallengesAuths.GetChallengesWithUnknownStatus();

        foreach (var challenge in challenges)
        {
            challenge.Status = ChallengeStatus.Rejected;
            await storage.ChallengesAuths.UpdateChallenge(challenge);
        }
    }

    public async Task<ChallengeAuthResult> WaitResultOfChallenge(ChallengeAuth challengeAuth, int attempts)
    {
        for (int i = 0; i < attempts; i++)
        {
            var actualChallenge = await storage.ChallengesAuths.GetChallengeById(challengeAuth.Id);
            if(actualChallenge is null) return new ChallengeAuthResult(false, "ChallengeId not found");

            switch (actualChallenge.Status)
            {
                case ChallengeStatus.Accept when actualChallenge.User is not null:
                    return new ChallengeAuthResult(true, "Успешная авторизация", 
                        new DetailsUser(telegramId:actualChallenge.User.Id, username:"", firstName:"", lastName:"",
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

    public async Task<ChallengeAuthResult> UpdateChallengeFromCallbackData(string challengeId,
        ChallengeStatus newStatus)
    {
        await RejectInActiveChallenges();
        var challenge = await storage.ChallengesAuths.GetChallengeById(challengeId);
        if (challenge is null) return new ChallengeAuthResult(false, $"ChallengeId #({challengeId}) not found");
        if(challenge.Status is ChallengeStatus.Accept or ChallengeStatus.Rejected) return new ChallengeAuthResult(false, "Время подтверждения истекло"); 
        challenge.Status = newStatus;
        await storage.ChallengesAuths.UpdateChallenge(challenge);
        return new ChallengeAuthResult(true, newStatus is ChallengeStatus.Accept ? $"Успешная авторизация в сервисе {challenge.Application?.Name}" : "Запрос на авторизацию отклонен");
    }
}