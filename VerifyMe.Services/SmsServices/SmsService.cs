using Telegram.Bot;
using VerifyMe.Models.DLA;
using VerifyMe.Models.DTO;
using VerifyMe.Models.DTO.Results;
using VerifyMe.Services.Extensions;
using VerifyMe.Storage;

namespace VerifyMe.Services.SmsServices;

public class SmsService(VerifyStorage storage, ITelegramBotClient botClient)
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

    public async Task<IList<Sms>> GetSmsByAppId(long appId)
    {
        return await storage.Sms.GetSmsByAppId(appId);
    }
}