using Telegram.Bot;
using Telegram.Bot.Types;

namespace VerifyMe.Telegram.Common;

public abstract class BaseCommand 
{
    public abstract string Command { get; }

    public bool Contains(Message message)
    {
        return message.Text != null && message.Text.Contains(Command);
    }

    public abstract Task ExecuteAsync(ITelegramBotClient client, Message message);
}