﻿using JetBrains.Annotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace blog.dto {
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class User : Identifiable {
        [Attr(Capabilities = AttrCapabilities.AllowSort | AttrCapabilities.AllowFilter)]
        public string Username { get; set; }
        public string Password { get; set; }
        [Attr(Capabilities = AttrCapabilities.AllowSort | AttrCapabilities.AllowFilter)]
        public string EMail { get; set; }
    }
}
