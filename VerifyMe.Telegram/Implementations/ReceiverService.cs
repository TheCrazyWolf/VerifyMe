using Microsoft.Extensions.Logging;
using Telegram.Bot;
using VerifyMe.Telegram.Abstractions;

namespace VerifyMe.Telegram.Implementations;

public class ReceiverService(ITelegramBotClient botClient, UpdateHandler updateHandler, ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);