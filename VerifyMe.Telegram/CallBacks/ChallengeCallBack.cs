using Telegram.Bot;
using Telegram.Bot.Types;
using VerifyMe.Telegram.Common;

namespace VerifyMe.Telegram.CallBacks;

public class ChallengeCallBack : BaseCallBackQuery
{
    public override string Name { get; set; } = "challenge_auth";
    
    public override void Execute(ITelegramBotClient client, CallbackQuery callbackQuery)
    {
        var array = TryGetArrayFromCallBack(callbackQuery);
        
        
        
        /*// example: schedule <type> <value> <date>
        if (callbackQuery.Message is null || array is null || array.Length == 0 ||
            !Enum.TryParse<ScheduleSearchType>(array[0], out var searchType) || !DateTime.TryParse(array[2], out var date))
        {
            if (callbackQuery.Message != null)
                await client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId,
                    replyMarkup: null);
            return;
        }
        
        var result = await clientSamgk.Schedule.GetScheduleAsync(DateOnly.FromDateTime(date), 
            searchType, array[1]);
        
        await client.TryEditMessage(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, 
            result.GetStringFromRasp(),
            replyMarkup: new InlineKeyboardMarkup(result.GenerateKeyboardOnSchedule(searchType, array[1])));*/
    }
}