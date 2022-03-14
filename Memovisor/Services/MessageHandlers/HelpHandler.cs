using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Memovisor.Services.MessageHandlers
{
    public class HelpHandler : IHandler
    {
        public async Task Handle(ITelegramBotClient botClient, Message message)
        {
            const string usage = "Короче:\n" +
                                     "/meme    - и УРЛ через пробел";

            await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                 text: usage,
                                                 replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
