using Telegram.Bot;
using Telegram.Bot.Types;

namespace Memovisor.Services.CommandHandlers
{
    public interface ICommandHandler
    {
        Task Handle(ITelegramBotClient botClient, Message message);
    }
}