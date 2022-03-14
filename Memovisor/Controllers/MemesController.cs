using Memovisor.Models;
using Memovisor.Services;
using Microsoft.AspNetCore.Mvc;

namespace Memovisor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemesController : ControllerBase
    {
        private readonly InMemoryImageStorage storage;

        public MemesController(InMemoryImageStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet("last")]
        public UrlDto GetLastMeme() => new UrlDto { Url = storage.GetLastMemeUrl() };
    }
}
