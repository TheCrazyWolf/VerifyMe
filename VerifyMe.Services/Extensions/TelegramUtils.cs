using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace VerifyMe.Services.Extensions;

public static class TelegramUtils
{
    public static async Task<bool> TrySendMessageAsync(this ITelegramBotClient client, long chatId, string message,
        IReplyMarkup? replyMarkup = null)
    {
        try
        {
            await client.SendTextMessageAsync(chatId, message, replyMarkup: replyMarkup, parseMode: ParseMode.Html);
            return true;
        }
        catch
        {
            return false;
        }
    }
}