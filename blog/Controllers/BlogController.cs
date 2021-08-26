using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;
using blog.dto;

namespace blog.Controllers {
    public sealed class BlogController : JsonApiController<BlogEntry> {
        private readonly ILogger<BlogController> _logger;

        public BlogController(IJsonApiOptions options, ILoggerFactory loggerFactory, IResourceService<BlogEntry> resourceService) : base(options, loggerFactory, resourceService) {
            _logger = new Logger<BlogController>(loggerFactory);
        }
    }
}

