using Telegram.Bot;
using Telegram.Bot.Types;

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
            var file = await botClient.GetFileAsync(message.Photo.Last().FileId);
            var extension = Path.GetExtension(file.FilePath);
            var fileName = $"/images/meme_{Guid.NewGuid()}{extension}";
            string filePath = $"wwwroot{fileName}";
            FileStream fs = new FileStream(filePath, FileMode.Create);
            await botClient.DownloadFileAsync(file.FilePath, fs);
            fs.Close();
            fs.Dispose();

            storage.SetLastMemeUrl(fileName);

            await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                 text: "Cool!");
        }
    }
}
