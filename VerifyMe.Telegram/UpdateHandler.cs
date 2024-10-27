using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VerifyMe.Telegram.Commands;
using VerifyMe.Telegram.Common;

namespace VerifyMe.Telegram;

public class UpdateHandler(ILogger<UpdateHandler> logger) : IUpdateHandler
{

    private IList<BaseCommand> _commands = new List<BaseCommand>()
    {
        new StartCommand()
    };
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        switch (update.Type)
        {
            case UpdateType.Message when update.Message?.From != null:
            {
                foreach (var command in _commands)
                {
                    if(!command.Contains(update.Message))
                        continue;
                    
                 //   logger.LogInformation($"Обработка команды: {command.Command}. от: ID {update.Message.From.Id} в чате: {update.Message.Chat.Id}");
                    await command.ExecuteAsync(botClient, update.Message);
                }
                //logger.LogInformation($"Сообщение: {update.Message.MessageId}. от: ID {update.Message.From.Id} в чате: {update.Message.Chat.Id} c текстом: {update.Message.Text}");
                break;
            }
            
            case UpdateType.Unknown:
                break;
            case UpdateType.InlineQuery:
                break;
            case UpdateType.ChosenInlineResult:
                break;
            case UpdateType.EditedMessage:
                break;
            case UpdateType.ChannelPost:
                break;
            case UpdateType.EditedChannelPost:
                break;
            case UpdateType.ShippingQuery:
                break;
            case UpdateType.PreCheckoutQuery:
                break;
            case UpdateType.Poll:
                break;
            case UpdateType.PollAnswer:
                break;
            case UpdateType.MyChatMember:
                break;
            case UpdateType.ChatMember:
                break;
            case UpdateType.ChatJoinRequest:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;;
    }
}