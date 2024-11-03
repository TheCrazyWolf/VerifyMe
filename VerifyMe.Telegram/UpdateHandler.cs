using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VerifyMe.Services.UsersServices;
using VerifyMe.Telegram.CallBacks;
using VerifyMe.Telegram.Commands;
using VerifyMe.Telegram.Common;

namespace VerifyMe.Telegram;

public class UpdateHandler(ILogger<UpdateHandler> logger, UsersService usersService, IServiceProvider serviceProvider) : IUpdateHandler
{
    private IList<BaseCommand> _commands = new List<BaseCommand>()
    {
        new StartCommand()
    };
    
    private IList<BaseCallBackQuery> _callBackQueries = new List<BaseCallBackQuery>()
    {
        new ChallengeCallBack(serviceProvider)
    };
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        switch (update.Type)
        {
            case UpdateType.Message when update.Message is { From: not null, Contact: not null }:
            {
                await new VerifyMeCommand(usersService).ExecuteAsync(botClient, update.Message);
                logger.LogInformation($"Сообщение: {update.Message.MessageId}. от: ID {update.Message.From.Id} в чате: {update.Message.Chat.Id} c текстом: {update.Message.Text}");logger.LogInformation($"Сообщение: {update.Message.MessageId}. от: ID {update.Message.From.Id} в чате: {update.Message.Chat.Id} c текстом: {update.Message.Contact.PhoneNumber}");
                break;
            }
            
            case UpdateType.Message when update.Message?.From != null:
            {
                foreach (var command in _commands)
                {
                    if(!command.Contains(update.Message))
                        continue;
                    
                    logger.LogInformation($"Обработка команды: {command.Command}. от: ID {update.Message.From.Id} в чате: {update.Message.Chat.Id}");
                    await command.ExecuteAsync(botClient, update.Message);
                }
                break;
            }
            
            case UpdateType.CallbackQuery when update.CallbackQuery is not null:
                foreach (var callBackQuery in _callBackQueries)
                {
                    if(!callBackQuery.Contains(update.CallbackQuery))
                        continue;
                    
                    logger.LogInformation($"Обработка команды: {callBackQuery.Name}. от: ID {update.CallbackQuery.From.Id}");
                    await callBackQuery.ExecuteAsync(botClient, update.CallbackQuery);
                }
                break;
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