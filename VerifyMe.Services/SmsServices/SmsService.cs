using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using VerifyMe.Models.DLA;
using VerifyMe.Models.DTO;
using VerifyMe.Models.DTO.Sms;
using VerifyMe.Models.Enums;
using VerifyMe.Services.Extensions;
using VerifyMe.Storage;

namespace VerifyMe.Services.SmsServices;

public class SmsService(VerifyStorage storage, ITelegramBotClient botClient)
{
    public async Task<SmsResult> SendSmsAsync(App app, string phoneNumber, string message, IReplyMarkup? replyMarkup = null)
    {
        phoneNumber = phoneNumber.GetNormalizedPhoneNumber();
        var user = await storage.Users.GetUserByPhoneAsync(phoneNumber);
        if (user == null) return new SmsResult(false, "Не удалось отправить сообщение. Пожалуйста перейдите, в телеграм бота и подтвердите свой номер телефона для получение смс-кодов.");
        if (string.IsNullOrEmpty(message)) return new SmsResult(false, "Сообщение не может быть пустым");
        message += $"\n\nℹ️ Вы получили это сообщение от сервиса: {app.Name}";
        var isSuccess = await botClient.TrySendMessageAsync(user.Id, message, replyMarkup: replyMarkup);
        Sms sms = new Sms { AppId = app.Id, UserId = user.Id, Message = message, IsDelivered  = isSuccess, DateTimeSend = DateTime.Now};
        await storage.Sms.CreateSmsAsync(sms);
        return new SmsResult(isSuccess, isSuccess ? "Сообщение успешно отправлено" : "Не удалость отправить сообщение. Пользователь деактивировал телеграм бота или проблемы с доступом к телеграму.");
    }
    
    public async Task<IList<SmsResultExtended>> SendSmsAsync(App app, IList<PostSendSms> dtos)
    {
        IList<SmsResultExtended> resultExtendeds = new List<SmsResultExtended>();
        foreach (var dto in dtos)
        {
            var result = await SendSmsAsync(app, dto.Phone, dto.Message);
            resultExtendeds.Add(new SmsResultExtended(result.IsSuccess, dto.Phone, result.SystemMessage));
            await Task.Delay(10000);
        }
        
        return resultExtendeds;
    }

    public async Task<IList<Sms>> GetSmsByAppIdAsync(long appId)
    {
        return await storage.Sms.GetSmsByAppIdAsync(appId);
    }

    public async Task<SmsResult> SendSmsRequestAuthAsync(App app, ChallengeAuth challengeAuth, User user)
    {
        return await SendSmsAsync(app: app, phoneNumber: user.PhoneNumber, 
            message: $"Получен запрос на авторизацию через сервис: {app.Name}. \n\nЭтому сервису будут доступны следующие данные: \n• Никнейм\n• Имя\n• Фамилия\n• Номер телефона",
            replyMarkup: new InlineKeyboardMarkup(GenerateKeyboard(challengeAuth)));
    }
    
    private IList<IList<InlineKeyboardButton>> GenerateKeyboard(ChallengeAuth challengeAuth)
    {
        return new List<IList<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("❌ Отклонить",
                    $"challenge_auth {challengeAuth.Id} {nameof(ChallengeStatus.Rejected)}"),
                InlineKeyboardButton.WithCallbackData("✅ Авторизоваться",
                    $"challenge_auth {challengeAuth.Id} {nameof(ChallengeStatus.Accept)}"),
            },
        };
    }
}