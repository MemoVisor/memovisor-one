using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Memovisor.Services.CommandHandlers
{
    public class ShowKeyboardCommand : ICommandHandler
    {
        public async Task Handle(ITelegramBotClient botClient, Message message)
        {
            var list = new List<KeyboardButton>()
            {
                new KeyboardButton("/get - Скачать мем"),
            };
            var markup = new ReplyKeyboardMarkup(list);
            markup.ResizeKeyboard = true;
            await botClient.SendTextMessageAsync(message.Chat.Id, "Что хочешь?", replyMarkup: markup);
        }
    }
}
