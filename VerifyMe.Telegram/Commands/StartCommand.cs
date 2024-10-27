using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VerifyMe.Telegram.Common;
using VerifyMe.Telegram.Extensions;

namespace VerifyMe.Telegram.Commands;

public class StartCommand : BaseCommand
{
    public override string Command { get; } = "/start";

    private string startMessage = "Вам необходимо подтвердить свой номер телефона " +
                                  "для получения кодов подтверждения в онлайн сервисах";

    public override async Task ExecuteAsync(ITelegramBotClient client, Message message)
    {
        KeyboardButton button = KeyboardButton.WithRequestContact("Подтвердить номер телефона");
        ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(button);
        keyboard.ResizeKeyboard = true;
        await client.TrySendMessage(message.Chat.Id, startMessage, replyMarkup: keyboard);
    }
}