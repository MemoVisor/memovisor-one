using Telegram.Bot;
using Telegram.Bot.Types;

namespace Memovisor.Services.MessageHandlers
{
    public interface IHandler
    {
        Task Handle(ITelegramBotClient botClient, Message message);
    }
}