using Telegram.Bot;
using VerifyMe.Models.DLA;
using VerifyMe.Services.Extensions;
using VerifyMe.Storage;

namespace VerifyMe.Services.SmsServices;

public class SmsService(VerifyStorage storage, ITelegramBotClient botClient)
{
    public async Task<int> SendSmsAsync(App app, string phoneNumber, string message)
    {
        var user = await storage.Users.GetUserByPhone(phoneNumber);
        if (user == null) return 404;
        message += $"\n\nℹ️ Вы получили это сообщение от сервиса: ID №{app.Id}. {app.Name}";
        var isSuccess = await botClient.TrySendMessage(user.Id, message);
        Sms sms = new Sms { AppId = app.Id, UserId = user.Id, Message = message, IsDelivered  = isSuccess, DateTimeSend = DateTime.Now};
        await storage.Sms.CreateSms(sms);
        return 200;
    }
    
    public async Task SendSmsAsync(App app, IList<string> phoneNumbers, string message)
    {
        foreach (var phoneNumber in phoneNumbers)
        {
            await SendSmsAsync(app, phoneNumber, message);
        }
    }
}