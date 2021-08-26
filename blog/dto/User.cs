﻿using JsonApiDotNetCore.Resources.Annotations;

namespace blog.dto {
    public class User {
        [Attr(Capabilities = AttrCapabilities.AllowSort | AttrCapabilities.AllowFilter)]
        public string Username { get; set; }
        public string Password { get; set; }
        [Attr(Capabilities = AttrCapabilities.AllowSort | AttrCapabilities.AllowFilter)]
        public string EMail { get; set; }
    }
}
