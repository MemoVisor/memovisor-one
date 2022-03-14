using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Memovisor.Services.MessageHandlers
{
    public class HelpHandler : IHandler
    {
        public async Task Handle(ITelegramBotClient botClient, Message message)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Короче:");
            builder.AppendLine("Можно скинуть фотку 📷");
            builder.AppendLine("Можно скинуть гифку 🖼");
            builder.AppendLine("Даже mp4-файл можно 🎥");
            builder.AppendLine("");
            builder.AppendLine("Можно смешное. Можно милое. Но давай только без большого текста😉");

            await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                 text: builder.ToString(),
                                                 replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
