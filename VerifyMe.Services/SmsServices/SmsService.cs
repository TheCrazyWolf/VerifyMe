﻿using Telegram.Bot;
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
        if (user == null) return new SmsResult(false, "Пользователь не зарегистрирован в сервисе");
        if (string.IsNullOrEmpty(message)) return new SmsResult(false, "Сообщение не может быть пустым");
        message += $"\n\nℹ️ Вы получили это сообщение от сервиса: {app.Name}";
        var isSuccess = await botClient.TrySendMessage(user.Id, message);
        Sms sms = new Sms { AppId = app.Id, UserId = user.Id, Message = message, IsDelivered  = isSuccess, DateTimeSend = DateTime.Now};
        await storage.Sms.CreateSms(sms);
        return new SmsResult(isSuccess, isSuccess ? "Сообщение доставлено" : "Пользователь удалил бота или телеграмм недоступен");
    }
    
    public async Task SendSmsAsync(App app, IList<string> phoneNumbers, string message)
    {
        foreach (var phoneNumber in phoneNumbers)
        {
            await SendSmsAsync(app, phoneNumber, message);
            await Task.Delay(10000);
        }
    }

    public async Task<IList<Sms>> GetSmsByAppId(long appId)
    {
        return await storage.Sms.GetSmsByAppId(appId);
    }
}