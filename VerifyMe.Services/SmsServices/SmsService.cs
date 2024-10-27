using Telegram.Bot;
using VerifyMe.Models.DLA;
using VerifyMe.Services.Extensions;
using VerifyMe.Storage;

namespace VerifyMe.Services.SmsServices;

public class SmsService(VerifyStorage storage, ITelegramBotClient botClient)
{
    public async Task<int> SendSmsAsync(string phoneNumber, long appId, string message)
    {
        var user = await storage.Users.GetUserByPhone(phoneNumber);
        if (user == null) return 404;

        var isSuccess = await botClient.TrySendMessage(user.Id, message);
        Sms sms = new Sms { AppId = appId, UserId = user.Id, Message = message, IsDelivered  = isSuccess, DateTimeSend = DateTime.Now};
        await storage.Sms.CreateSms(sms);
        return 200;
    }
    
    public async Task SendSmsAsync(IList<string> phoneNumbers, long appId, string message)
    {
        foreach (var phoneNumber in phoneNumbers)
        {
            await SendSmsAsync(phoneNumber, appId, message);
        }
    }
}