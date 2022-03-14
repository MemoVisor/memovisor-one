using Memovisor.Services.CommandHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Memovisor.Services.MessageHandlers
{
    public class CommandsHandler : IHandler
    {
        private readonly IServiceProvider serviceProvider;

        public CommandsHandler(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task Handle(ITelegramBotClient botClient, Message message)
        {
            ICommandHandler? command = null;
            
            if (message.Text.StartsWith("/get"))
            {
                command = serviceProvider.GetRequiredService<GetCommandHandler>();
            }

            if (command == null)
            {
                return Task.CompletedTask;
            }

            return command.Handle(botClient, message);
        }
    }
}
