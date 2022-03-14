using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace Memovisor.Services
{
    public class TelegramService
    {
        private readonly TelegramBotClient? Bot;
        private readonly Handlers handlers;

        public TelegramService(string token, Handlers handlers)
        {
            Bot = new TelegramBotClient(token);

            ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
            Bot.StartReceiving(handlers.HandleUpdateAsync,
                               handlers.HandleErrorAsync,
                               receiverOptions);
            this.handlers = handlers;
        }
    }
}
