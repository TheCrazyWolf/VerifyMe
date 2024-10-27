using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VerifyMe.Telegram.Common;
using VerifyMe.Telegram.Extensions;

namespace VerifyMe.Telegram.Commands;

public class VerifyMeCommand : BaseCommand
{
    public override string Command { get; } = string.Empty;

    private readonly string _successVerifyed =
        "Спасибо, номер подтвержден. Теперь Вы сможете получать сообщения от онлайн-сервисов, которые подключены к этому боту.";

    private readonly string _unSuccessVerifyed =
        "К сожалению, Вы пытаетесь надурить. Как надумаете подтвердить номер отправьте команду /start";

    public override async Task ExecuteAsync(ITelegramBotClient client, Message message)
    {
        if (message.From?.Id != message.Contact?.UserId)
        {
            await client.TrySendMessage(message.Chat.Id, _unSuccessVerifyed, replyMarkup: new ReplyKeyboardRemove());
            return;
        }

        await client.TrySendMessage(message.Chat.Id, _successVerifyed, replyMarkup: new ReplyKeyboardRemove());
    }
}