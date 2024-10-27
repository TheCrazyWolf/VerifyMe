using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace VerifyMe.Telegram.Extensions;

public static class TelegramUtils
{
    public static async Task<bool> TrySendMessage(this ITelegramBotClient client, long chatId, string message, IReplyMarkup? replyMarkup = null)
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
    
    public static async Task<bool> TryEditMessage(this ITelegramBotClient client,
        long chatId, int messageIdm, string message, IReplyMarkup? replyMarkup = null)
    {
        try
        {
            await client.EditMessageTextAsync(chatId: chatId, messageId: messageIdm, message,
                replyMarkup: replyMarkup as InlineKeyboardMarkup,
                parseMode: ParseMode.Html);
            return true;
        }
        catch 
        {
            return false;
        }
    }
    
    public static async Task<bool> TryDeleteMessage(this ITelegramBotClient client, long chatId, int messageId)
    {
        try
        {
            await client.DeleteMessageAsync(chatId, messageId);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public static async Task<bool> TrySendDocument(this ITelegramBotClient client, long chatId, InputFile file)
    {
        try
        {
            await client.SendDocumentAsync(chatId, file);
            return true;
        }
        catch 
        {
            return false;
        }
    }
}