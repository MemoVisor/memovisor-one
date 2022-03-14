using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Memovisor.Services.MessageHandlers
{
    public class PhotoHandler : IHandler
    {
        private readonly InMemoryImageStorage storage;

        public PhotoHandler(InMemoryImageStorage storage)
        {
            this.storage = storage;
        }

        public async Task Handle(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.UploadDocument);
            var file = await botClient.GetFileAsync(message.Photo.Last().FileId);
            var fileName = await botClient.DownloadFile(file);
            storage.SetLastMemeUrl(fileName);
            await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                 text: "Cool!");
        }
    }
}
