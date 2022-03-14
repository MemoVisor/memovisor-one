using Telegram.Bot;
using Telegram.Bot.Types;

namespace Memovisor.Services.MessageHandlers
{
    public class UrlHandler : IHandler
    {
        private readonly InMemoryImageStorage storage;

        public UrlHandler(InMemoryImageStorage storage)
        {
            this.storage = storage;
        }

        public async Task Handle(ITelegramBotClient botClient, Message message)
        {
            var url = message.Text;
            await Validate(botClient, message, url);

            storage.SetLastMemeUrl(url);
            await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                 text: "Nice!");
        }

        private static async Task Validate(ITelegramBotClient botClient, Message message, string? url)
        {
            try
            {
                new Uri(url);
            }
            catch
            {
                await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                     text: "Ты чё? Это не урл!");
                throw;
            }
        }
    }
}
