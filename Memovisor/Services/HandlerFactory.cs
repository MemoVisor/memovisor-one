using Memovisor.Services.MessageHandlers;
using Telegram.Bot.Types;

namespace Memovisor.Services
{
    public class HandlerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public HandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IHandler GetHandler(Message message)
        {
            if (message.Photo != null)
            {
                return serviceProvider.GetRequiredService<PhotoHandler>();
            }
            if (message.Document != null)
            {
                return serviceProvider.GetRequiredService<DocumentHandler>();
            }
            if (message.Text.StartsWith("http"))
            {
                return serviceProvider.GetRequiredService<UrlHandler>();
            }
            return serviceProvider.GetRequiredService<HelpHandler>();
        }
    }
}
