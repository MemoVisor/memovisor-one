using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Memovisor.Services.MessageHandlers
{
    public class DocumentHandler : IHandler
    {
        private readonly InMemoryImageStorage storage;

        public DocumentHandler(InMemoryImageStorage storage)
        {
            this.storage = storage;
        }

        public async Task Handle(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.UploadDocument);
            var file = await botClient.GetFileAsync(message.Document.FileId);
            if (Path.GetExtension(file.FilePath) != ".mp4")
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Ненене! Такой файл не подходит. Расшерение .mp4 надо");
                return;
            }

            var fileName = await botClient.DownloadFile(file);
            storage.SetLastMemeUrl(fileName);
            await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                 text: "Woow!");
        }
    }
}
