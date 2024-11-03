using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VerifyMe.Models.Enums;
using VerifyMe.Services.AuthServices;
using VerifyMe.Storage;
using VerifyMe.Storage.Context;
using VerifyMe.Telegram.Common;
using VerifyMe.Telegram.Extensions;

namespace VerifyMe.Telegram.CallBacks;

public class ChallengeCallBack(IServiceProvider serviceProvider) : BaseCallBackQuery
{
    public override string Name { get; set; } = "challenge_auth";
    
    public override async Task Execute(ITelegramBotClient client, CallbackQuery callbackQuery)
    {
        var array = TryGetArrayFromCallBack(callbackQuery);

        if (callbackQuery.Message is null || array is null ||
            !Enum.TryParse<ChallengeStatus>(array[1], out var challengeStatus))
        {
            return;
        }
        
        /*var authService = serviceProvider.GetService<AuthService>(); if(authService is null) return;*/
        var authService = new AuthService(new VerifyStorage(new VerifyContext()));
        var result = await authService.UpdateChallengeFromCallbackData(array[0], challengeStatus);

        await client.TryEditMessage(chatId: callbackQuery.Message.Chat.Id, messageId: callbackQuery.Message.MessageId,
            message: result.SystemMessage, new ReplyKeyboardRemove());
    }
}