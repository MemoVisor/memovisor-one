namespace Memovisor.Services
{
    public class InMemoryImageStorage
    {
        private readonly string baseUrl;

        private static string lastMemeUrl = "images/meme.png";

        public InMemoryImageStorage(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public string GetLastMemeUrl() => Path.Combine(baseUrl, lastMemeUrl);

        public string LocalMemeUrl => Path.Combine("wwwroot", lastMemeUrl);

        public void SetLastMemeUrl(string url) => lastMemeUrl = url.Trim('/');
    }
}
