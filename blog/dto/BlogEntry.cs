using System;
using JetBrains.Annotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace blog.dto {
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class BlogEntry : Identifiable {
        [Attr(Capabilities = AttrCapabilities.AllowSort | AttrCapabilities.AllowView)]
        public DateTime Date { get; set; }
        [Attr]
        public string Content { get; set; }
        [HasOne]
        public User User { get; set; }
    }
}
