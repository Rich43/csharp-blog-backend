using System;
using JetBrains.Annotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace blog.dto {
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class BlogEntry : Identifiable {
        [Attr(Capabilities = AttrCapabilities.AllowSort)]
        public DateTime Date { get; set; }
        [Attr]
        public string Content { get; set; }
        [Attr, HasOne]
        public User User { get; set; }
    }
}
