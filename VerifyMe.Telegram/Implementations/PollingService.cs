using Microsoft.Extensions.Logging;
using VerifyMe.Telegram.Abstractions;

namespace VerifyMe.Telegram.Implementations;

public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);