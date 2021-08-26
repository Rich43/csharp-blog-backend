using System;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace blog.dto {
    public sealed class BlogEntry : Identifiable {
        [Attr(Capabilities = AttrCapabilities.AllowSort)]
        public DateTime Date { get; set; }
        [Attr]
        public string Content { get; set; }
        [Attr, HasOne]
        public User User { get; set; }
    }
}
