using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Memovisor.Services.CommandHandlers
{
    public class GetCommandHandler : ICommandHandler
    {
        private readonly InMemoryImageStorage storage;

        public GetCommandHandler(InMemoryImageStorage storage)
        {
            this.storage = storage;
        }

        public async Task Handle(ITelegramBotClient botClient, Message message)
        {
            var path = storage.LocalMemeUrl;
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var fileName = Path.GetFileName(path);

            if (Path.GetExtension(path) == ".mp4")
            {
                await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.UploadDocument);
                await botClient.SendDocumentAsync(message.Chat.Id,
                    new InputOnlineFile(fileStream, fileName));
            }
            else
            {
                await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);
                await botClient.SendPhotoAsync(message.Chat.Id,
                    new InputOnlineFile(fileStream, fileName));
            }
        }
    }
}
