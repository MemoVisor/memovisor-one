using Telegram.Bot;

namespace Memovisor.Services.MessageHandlers
{
    public static class FileDownloader
    {
        public async static Task<string> DownloadFile(this ITelegramBotClient botClient, Telegram.Bot.Types.File file)
        {
            var extension = Path.GetExtension(file.FilePath);
            var fileName = $"/images/meme_{Guid.NewGuid()}{extension}";
            string filePath = $"wwwroot{fileName}";
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            await botClient.DownloadFileAsync(file.FilePath, fs);

            return fileName;
        }
    }
}
