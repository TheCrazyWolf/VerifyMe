using Telegram.Bot;
using VerifyMe.Models.DLA;
using VerifyMe.Models.DTO.ChallengeAuth;
using VerifyMe.Models.DTO.Sms;
using VerifyMe.Models.Enums;
using VerifyMe.Services.Extensions;
using VerifyMe.Storage;

namespace VerifyMe.Services.AuthServices;

public class AuthService(VerifyStorage storage, ITelegramBotClient botClient)
{
    
    public async Task<SmsResult> SendSmsAsync(App app, string phoneNumber, string message)
    {
        phoneNumber = phoneNumber.GetNormalizedPhoneNumber();
        var user = await storage.Users.GetUserByPhone(phoneNumber);
        if (user == null) return new SmsResult(false, "Не удалось отправить сообщение. Пожалуйста перейдите, в телеграм бота и подтвердите свой номер телефона для получение смс-кодов.");
        if (string.IsNullOrEmpty(message)) return new SmsResult(false, "Сообщение не может быть пустым");
        message += $"\n\nℹ️ Вы получили это сообщение от сервиса: {app.Name}";
        var isSuccess = await botClient.TrySendMessage(user.Id, message);
        Sms sms = new Sms { AppId = app.Id, UserId = user.Id, Message = message, IsDelivered  = isSuccess, DateTimeSend = DateTime.Now};
        await storage.Sms.CreateSms(sms);
        return new SmsResult(isSuccess, isSuccess ? "Сообщение успешно отправлено" : "Не удалость отправить сообщение. Пользователь деактивировал телеграм бота или проблемы с доступом к телеграму.");
    }

    public async Task<User?> GetUserByPhoneNumber(string dtoPhone)
    {
        return await storage.Users.GetUserByPhone(dtoPhone);
    }

    public async Task<ChallengeAuth> CreateChallengeAuth(App application, User user)
    {
        var challenge = new ChallengeAuth()
        {
            User = user,
            UserId = user.Id,
            Application = application,
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
            var actualChallenge = await storage.ChallengesAuths.GetChallengeById(challengeAuth.Id.ToString());
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
}