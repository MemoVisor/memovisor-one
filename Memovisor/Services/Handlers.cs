using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Memovisor.Services
{
    public class Handlers
    {
        private readonly InMemoryImageStorage storage;

        public Handlers(InMemoryImageStorage storage)
        {
            this.storage = storage;
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!),
                UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery!),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Photo != null)
            {
                await SavePhoto(botClient, message);
                return;
            }
            if (message.Document != null)
            {
                await SaveDocument(botClient, message);
                return;
            }
            if (message.Type != MessageType.Text)
                return;

            var action = message.Text!.Split(' ')[0] switch
            {
                "/meme" => SendUrl(botClient, message),
                _ => Usage(botClient, message)
            };

            Message sentMessage = await action;

            async Task<Message> SendUrl(ITelegramBotClient botClient, Message message)
            {
                await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                if (message.Text!.Split(' ').Length < 2)
                {
                    return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                                text: "Ты чё? где урл?");
                }
                var url = message.Text!.Split(' ')[1];
                storage.SetLastMemeUrl(url);

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Nice!");
            }

            static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
            {
                const string usage = "Короче:\n" +
                                     "/meme    - и УРЛ через пробел";

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: usage,
                                                            replyMarkup: new ReplyKeyboardRemove());
            }

            async Task<Message> SavePhoto(ITelegramBotClient botClient, Message message)
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

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: "Cool!");
            }
        }

        private async Task<Message> SaveDocument(ITelegramBotClient botClient, Message message)
        {
            var file = await botClient.GetFileAsync(message.Document.FileId);
            var extension = Path.GetExtension(file.FilePath);
            var fileName = $"/images/meme_{Guid.NewGuid()}{extension}";
            string filePath = $"wwwroot{fileName}";
            FileStream fs = new FileStream(filePath, FileMode.Create);
            await botClient.DownloadFileAsync(file.FilePath, fs);
            fs.Close();
            fs.Dispose();

            storage.SetLastMemeUrl(fileName);

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Woow!");
        }

        // Process Inline Keyboard callback data
        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }

        private static async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            Console.WriteLine($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "3",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };

            await botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                   results: results,
                                                   isPersonal: true,
                                                   cacheTime: 0);
        }

        private static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}
