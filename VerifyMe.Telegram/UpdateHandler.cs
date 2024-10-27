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
            case UpdateType.Message when update.Message is { From: not null, Contact: not null }:
            {
                await new VerifyMeCommand().ExecuteAsync(botClient, update.Message);
                break;
            }
            
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
            case UpdateType.InlineQuery:
            case UpdateType.ChosenInlineResult:
            case UpdateType.EditedMessage:
            case UpdateType.ChannelPost:
            case UpdateType.EditedChannelPost:
            case UpdateType.ShippingQuery:
            case UpdateType.PreCheckoutQuery:
            case UpdateType.Poll:
            case UpdateType.PollAnswer:
            case UpdateType.MyChatMember:
            case UpdateType.ChatMember:
            case UpdateType.ChatJoinRequest:
                break;
        }
    }
    

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;;
    }
}